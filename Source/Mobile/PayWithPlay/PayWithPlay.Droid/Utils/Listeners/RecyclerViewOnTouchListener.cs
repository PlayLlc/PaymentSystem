using Android.Views;
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class RecyclerViewOnTouchListener : Java.Lang.Object, IOnItemTouchListener
    {
        public bool OnInterceptTouchEvent(RecyclerView recyclerView, MotionEvent e)
        {
            var action = e.Action;
            switch (action)
            {
                case MotionEventActions.Down:
                    // Disallow RecyclerView to intercept touch events.
                    recyclerView.Parent!.RequestDisallowInterceptTouchEvent(true);
                    break;
            }

            return false;
        }

        public void OnRequestDisallowInterceptTouchEvent(bool disallow) { }

        public void OnTouchEvent(RecyclerView recyclerView, MotionEvent @event) { }
    }
}