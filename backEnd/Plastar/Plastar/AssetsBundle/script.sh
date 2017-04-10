#!/bin/bash

# Unity path
# project path
# resource path
# assets bundle path



UNITY_EXEC=/c/"Program Files"/Unity/Editor/Unity.exe

# if [ -z "$2" ]; then echo "You must provide a path to the bundle assets and a path to the resulting bundle."; fi

CREATION_TIME=`date +%s` ASSET_BUNDLE_PROJECT_DIR=/c/TangoProject/sample/recognize/backEnd/Plastar/Plastar/AssetsBundle/tmp/AssetBundle-${CREATION_TIME}

echo $ASSET_BUNDLE_PROJECT_DIR

echo "Creating temporary project."; "${UNITY_EXEC}" -batchmode -quit -createProject ${ASSET_BUNDLE_PROJECT_DIR};



RESOURCE_FILE=/c/TangoProject/sample/recognize/backEnd/Plastar/Plastar/AssetsBundle/resource

echo "Copying resources from source folder to assets folder."; cd $RESOURCE_FILE; cp -r . ${ASSET_BUNDLE_PROJECT_DIR}/Assets;

echo "Finding assets."; cd ${ASSET_BUNDLE_PROJECT_DIR}; ASSETS_TO_BUNDLE=`find Assets -type f -name "*.*" | sed 's/^.\///g' | sed 's/^/assetPathsList.Add("/g' | sed 's/$/");/g'`

echo $ASSETS_TO_BUNDLE




mkdir ${ASSET_BUNDLE_PROJECT_DIR}/Assets/Editor/
cat << EOF > ${ASSET_BUNDLE_PROJECT_DIR}/Assets/Editor/AssetsBundler.cs

using UnityEditor;
using System.Collections.Generic;

public class AssetsBundler
{
    [MenuItem("Assets/BuildAllAssetBundles")]
    static void BuildAllAssetBundles()
    {
        List<string> assetPathsList = new List<string>();
        ${ASSETS_TO_BUNDLE}

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        buildMap[0].assetBundleName = "testBundle";

        string[] enemyAssets = new string[assetPathsList.Count];

        for (int i = 0; i < assetPathsList.Count; i++)
            enemyAssets[i] = assetPathsList[i];

        buildMap[0].assetNames = enemyAssets;

        BuildPipeline.BuildAssetBundles("C:/TangoProject/sample/recognize/backEnd/Plastar/Plastar/AssetsBundle/AssetsBundle", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}

EOF



echo "Building the bundle."; "${UNITY_EXEC}" -batchmode -projectPath ${ASSET_BUNDLE_PROJECT_DIR} -executeMethod AssetsBundler.BuildAllAssetBundles -quit;

echo "Deleting temporary project."; rm -rf ${ASSET_BUNDLE_PROJECT_DIR};

read -p "Press enter to continue"