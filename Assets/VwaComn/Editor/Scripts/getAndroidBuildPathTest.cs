using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
/*
* 
* ArgumentException: Illegal characters in path.
System.IO.Path.Combine (System.String path1, System.String path2) (at /Users/builduser/buildslave/mono/build/mcs/class/corlib/System.IO/Path.cs:124)
UnityEditor.Android.AndroidSDKTools.BuildToolsExe (System.String command)
UnityEditor.Android.AndroidSDKTools.get_AAPT ()
UnityEditor.Android.AndroidSDKTools.DumpDiagnostics ()
UnityEditor.Android.PostProcessor.Tasks.CheckAndroidSdk.Execute (UnityEditor.Android.PostProcessor.PostProcessorContext context)
UnityEditor.Android.PostProcessor.PostProcessRunner.RunAllTasks (UnityEditor.Android.PostProcessor.PostProcessorContext context)
UnityEditor.Android.PostProcessAndroidPlayer.PostProcess (BuildTarget target, System.String stagingAreaData, System.String stagingArea, System.String playerPackage, System.String installPath, System.String companyName, System.String productName, BuildOptions options, UnityEditor.RuntimeClassRegistry usedClassRegistry)
UnityEditor.Android.AndroidBuildPostprocessor.PostProcess (BuildPostProcessArgs args)
UnityEditor.PostprocessBuildPlayer.Postprocess (BuildTarget target, System.String installPath, System.String companyName, System.String productName, Int32 width, Int32 height, System.String downloadWebplayerUrl, System.String manualDownloadWebplayerUrl, BuildOptions options, UnityEditor.RuntimeClassRegistry usedClassRegistry, UnityEditor.BuildReporting.BuildReport report) (at C:/buildslave/unity/build/Editor/Mono/BuildPipeline/PostprocessBuildPlayer.cs:176)
UnityEditor.HostView:OnGUI()
*/
[InitializeOnLoad]

public class getAndroidBuildPathTest : MonoBehaviour {
    [MenuItem("VWA Debug/Test Get Android Build Path")]
    static void testandroidbuild() {
        Debug.Log("test");
        
        
        Assembly [] a = AppDomain.CurrentDomain.GetAssemblies();
        Assembly androidsdktools = a.First(z => z.FullName == "UnityEditor.Android.Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        Debug.Log(androidsdktools);
        Type sdk = androidsdktools.GetType("UnityEditor.Android.AndroidSDKTools");
        Type java = androidsdktools.GetType("UnityEditor.Android.AndroidJavaTools");
        Debug.Log(sdk);
        Debug.Log(java);

        String[] args = { "aapt" };
        object cursdk = sdk.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

        Debug.Log("AndroidJavaTools.exe(\"aapt\"): " + java.GetMethod("Exe", BindingFlags.Public | BindingFlags.Static).Invoke(null, args));
        Debug.Log("SDKBuildToolsDir: >>>" + sdk.GetField("SDKBuildToolsDir",BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cursdk)+ "<<<");
        Debug.Log("BuildToolsExe(\"aapt\"): " + sdk.GetMethod("BuildToolsExe", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(cursdk, args));
        Debug.Log("get_AAPT(): " + sdk.GetProperty("AAPT").GetValue(cursdk, null));
    }

}

