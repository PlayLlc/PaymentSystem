using Android.Content.PM;
using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.Activity.Result;
using AndroidX.Camera.Core;
using AndroidX.Camera.Lifecycle;
using AndroidX.Camera.View;
using AndroidX.Core.Content;
using Google.Common.Util.Concurrent;
using Java.Lang;
using Java.Util.Concurrent;
using MvvmCross.ViewModels;
using PayWithPlay.Droid.Utils.Scanner;
using static Android.Views.ViewGroup;
using Android;
using AndroidX.Activity.Result.Contract;
using PayWithPlay.Droid.Utils.Callbacks;

namespace PayWithPlay.Droid.Fragments
{
    public abstract class BaseScanFragment<TViewModel> : BaseFragment<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected string? _currentScanResult;
        protected FrameLayout? _cameraPreviewContainer;
        protected PreviewView? _cameraPreview;
        protected IListenableFuture? _cameraProviderFuture;
        protected IExecutorService? _cameraExecutor;
        protected ProcessCameraProvider? _cameraProvider;
        protected CameraSelector? _cameraSelector;
        protected ImageAnalysis? _imageAnalysis;
        private bool _isScanning;

        private ActivityResultLauncher? _requestCameraPermissionLauncher;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(view);


            return view;
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) == Permission.Granted)
            {
                InitAndStartCamera();
            }
            else 
            {
                _requestCameraPermissionLauncher = RegisterForActivityResult(new ActivityResultContracts.RequestPermission(), new ActivityResultCallback((result) =>
                {
                    if (result is Java.Lang.Boolean boolResult)
                    {
                        if ((bool)boolResult)
                        {
                            InitAndStartCamera();
                        }
                        else
                        {
                            // TODO:: handle no permission, maybe display a pop up message
                        }
                    }
                }));
                _requestCameraPermissionLauncher.Launch(Manifest.Permission.Camera);
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            StopScanning();
            _cameraExecutor?.Shutdown();
        }

        public void StopScanning()
        {
            if (!_isScanning)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                _imageAnalysis?.ClearAnalyzer();
                _cameraProvider?.UnbindAll();
                _currentScanResult = null;
                _isScanning = false;
            }, null);
        }

        public void StartScanning()
        {
            if (_isScanning)
            {
                return;
            }

            BindPreview(_cameraProvider);
        }

        protected abstract void OnResult(string result);

        protected void BindPreview(ProcessCameraProvider cameraProvider)
        {
            _isScanning = true;

            cameraProvider?.UnbindAll();

            var preview = new Preview.Builder().Build();

            _cameraSelector = new CameraSelector.Builder()
                .RequireLensFacing(CameraSelector.LensFacingBack)
                .Build();

            _imageAnalysis = new ImageAnalysis.Builder()
                .SetTargetResolution(new Size(_cameraPreview!.Width, _cameraPreview.Height))
                .SetBackpressureStrategy(ImageAnalysis.StrategyKeepOnlyLatest)
                .Build();

            var analyzer = new ZXingBarcodeAnalyzer((result) =>
            {
                if (string.IsNullOrWhiteSpace(result) || result.Equals(_currentScanResult))
                {
                    return;
                }

                _currentScanResult = result;

                OnResult(result);
                // OnResult
            });

            _imageAnalysis.SetAnalyzer(_cameraExecutor, analyzer);

            preview.SetSurfaceProvider(_cameraPreview.SurfaceProvider);

            var camera = cameraProvider?.BindToLifecycle(this, _cameraSelector, _imageAnalysis, preview);
        }

        protected virtual void InitViews(View root)
        {
            _cameraPreviewContainer = root.FindViewById<FrameLayout>(Resource.Id.camera_preview_container)!;
            _cameraPreview = root.FindViewById<PreviewView>(Resource.Id.cameraPreview)!;

            var lp = _cameraPreviewContainer.LayoutParameters as MarginLayoutParams;
            lp!.Height = (int)(Resources!.DisplayMetrics!.WidthPixels * 0.77f);
            _cameraPreviewContainer.LayoutParameters = lp;
        }

        private void InitAndStartCamera() 
        {
            _cameraProviderFuture = ProcessCameraProvider.GetInstance(Context);
            _cameraExecutor = Executors.NewSingleThreadExecutor();

            _cameraProviderFuture.AddListener(new Runnable(() =>
            {
                _cameraProvider = (ProcessCameraProvider)_cameraProviderFuture.Get()!;
                BindPreview(_cameraProvider);

            }), ContextCompat.GetMainExecutor(Context));

        }
    }
}
