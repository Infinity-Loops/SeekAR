using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.iOS.Xcode.Extensions;

public class IOSPostProcessBuild
{
    [PostProcessBuild(100)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            // Modifica o Info.plist
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NSHealthShareUsageDescription", "Health data usage is required for in-game mechanics");
            rootDict.SetString("NSHealthUpdateUsageDescription", "Health update usage is required for in-game mechanics");

            File.WriteAllText(plistPath, plist.WriteToString());

            // Modifica o projeto Xcode
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            string targetGuid = proj.GetUnityMainTargetGuid();

            // Define o caminho para o arquivo de entitlements
            string entitlementsFileName = "GeoquestAR.entitlements";
            string entitlementsFilePath = Path.Combine(path, entitlementsFileName);

            // Cria ou carrega o arquivo de entitlements
            PlistDocument entitlements = new PlistDocument();
            if (File.Exists(entitlementsFilePath))
            {
                entitlements.ReadFromFile(entitlementsFilePath);
            }

            PlistElementDict entitlementsRoot = entitlements.root;

            // Adiciona as capacidades ao arquivo de entitlements
            entitlementsRoot.SetBoolean("com.apple.developer.kernel.increased-memory-limit", true);
            entitlementsRoot.SetBoolean("com.apple.developer.kernel.extended-virtual-addressing", true);

            // Salva o arquivo de entitlements
            entitlements.WriteToFile(entitlementsFilePath);

            // Configura o projeto para usar o arquivo de entitlements
            proj.AddFile(entitlementsFileName, entitlementsFileName);
            proj.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsFileName);

            // Salva as alterações no projeto Xcode
            proj.WriteToFile(projPath);
        }
    }
}
