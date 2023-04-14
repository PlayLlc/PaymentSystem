using Android.Util;
using Android.Views;
using AndroidX.Camera.Core;
using AndroidX.Camera.Lifecycle;
using AndroidX.Core.Content;
using Google.Common.Util.Concurrent;
using Java.Lang;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using AndroidX.Camera.View;
using static Android.Views.ViewGroup;
using PayWithPlay.Droid.Utils.Scanner;
using Java.Util.Concurrent;
using PayWithPlay.Droid.Extensions;
using AndroidX.AppCompat.Widget;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ScanItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_scan_item)]
    public class ScanItemFragment : BaseFragment<ScanItemViewModel>
    {
        private string? _currentScanResult;
        private FrameLayout? _cameraPreviewContainer;
        private PreviewView? _cameraPreview;
        private IListenableFuture? _cameraProviderFuture;
        private IExecutorService? _cameraExecutor;
        private ProcessCameraProvider? _cameraProvider;

        public override int LayoutId => Resource.Layout.fragment_inventory_scan_item;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnNewScanAction = OnNewScan;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(view);

            _cameraProviderFuture = ProcessCameraProvider.GetInstance(Context);
            _cameraExecutor = Executors.NewSingleThreadExecutor();

            _cameraProviderFuture.AddListener(new Runnable(() =>
            {
                _cameraProvider = (ProcessCameraProvider)_cameraProviderFuture.Get()!;
                BindPreview(_cameraProvider);

            }), ContextCompat.GetMainExecutor(Context));

            return view;
        }

        public override void OnStop()
        {
            base.OnStop();

            ViewModel.OnNewScanAction = null;
        }

        private void OnNewScan()
        {
            BindPreview(_cameraProvider);
        }

        private void BindPreview(ProcessCameraProvider cameraProvider)
        {
            cameraProvider?.UnbindAll();

            var preview = new Preview.Builder().Build();

            var cameraSelector = new CameraSelector.Builder()
                .RequireLensFacing(CameraSelector.LensFacingBack)
                .Build();

            var imageAnalysis = new ImageAnalysis.Builder()
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

                RequireActivity().RunOnUiThread(() => 
                {
                    imageAnalysis.ClearAnalyzer();
                    cameraProvider?.UnbindAll();
                    _currentScanResult = null;
                });

                ViewModel.OnScanResult(result);
                // OnResult
            });

            imageAnalysis.SetAnalyzer(_cameraExecutor, analyzer);

            preview.SetSurfaceProvider(_cameraPreview.SurfaceProvider);

            var camera = cameraProvider?.BindToLifecycle(this, cameraSelector, imageAnalysis, preview);
        }

        private void InitViews(View root)
        {
            _cameraPreviewContainer = root.FindViewById<FrameLayout>(Resource.Id.camera_preview_container)!;
            _cameraPreview = root.FindViewById<PreviewView>(Resource.Id.cameraPreview)!;
            var resultsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.result_container)!;

            resultsContainer.SetBackground(Resource.Color.white, topLeft: 5f.ToPx(), topRight: 5f.ToPx());

            var lp = _cameraPreviewContainer.LayoutParameters as MarginLayoutParams;
            lp!.Height = (int)(Resources!.DisplayMetrics!.WidthPixels * 0.77f);
            _cameraPreviewContainer.LayoutParameters = lp;
        }
    }
}
