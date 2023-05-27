-keep class com.microsoft.appcenter.** {*;}
-dontwarn com.microsoft.appcenter.**

-keepnames class * implements com.google.android.material.button.MaterialButton {
    public static final ** CREATOR;
}

-keepnames class * implements androidx.fragment.app.FragmentState {
    public static final ** CREATOR;
}