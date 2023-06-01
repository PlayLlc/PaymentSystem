-keep class com.microsoft.appcenter.** {*;}
-dontwarn com.microsoft.appcenter.**

-keep class cards.pay.paycardsrecognizer.sdk.ndk.RecognitionCoreNdk {*;}
-dontwarn cards.pay.paycardsrecognizer.sdk.ndk.RecognitionCoreNdk

-keepclassmembers class * implements android.os.Parcelable {
    public static final ** CREATOR;
}