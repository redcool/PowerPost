1 create PowerPostData
    Project click "Assets/Create/Rendering/Universal Render Pipeline/PowerPostData"
    add PostExSettings's fullname to PowerPostData's Setting Names
2 add PowerPostFeature to URP Asset.
    add PowerPostData to PowerPostFeature's assetData
3 add PowerPostEffect to urp Post's volume

4 write custom powerpost effect.
    1 create a script extends PostExSettings
        1.1add attributes [Serializable,VolumeComponentMenu("Custom/SSSS")]
    2 create a script extends PostExPass
    3 register this effect to PowerPostData.asset