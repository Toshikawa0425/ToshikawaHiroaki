// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Charactor/No_TR" {
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

        CGINCLUDE
#define UNITY_SETUP_BRDF_INPUT MetallicSetup
            ENDCG

            SubShader
        {
            Tags {"RenderType" = "TransparentCutout"  "Queue" = "Geometry+1"}

            //ÉLÉÉÉâÇ™å©Ç¶ÇƒÇ¢ÇÈïîï™ÇÇRÇ…ÇµÅAï`âÊ
            Pass
            {
                Tags {"LightMode" = "ForwardBase" }
                

                CGPROGRAM
                #pragma target 3.0

            // -------------------------------------

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_fragment _EMISSION
            #pragma shader_feature_local _METALLICGLOSSMAP
            #pragma shader_feature_local_fragment _DETAIL_MULX2
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature_local _PARALLAXMAP

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
            //#pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma vertex vertBase
            #pragma fragment fragBase
            #include "UnityStandardCoreForward.cginc"

            ENDCG

        }
            

        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 3.0

                // -------------------------------------


                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma shader_feature_local _PARALLAXMAP
                #pragma multi_compile_shadowcaster
                #pragma multi_compile_instancing
                // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
                //#pragma multi_compile _ LOD_FADE_CROSSFADE

                #pragma vertex vertShadowCaster
                #pragma fragment fragShadowCaster

                #include "UnityStandardShadow.cginc"

                ENDCG
            }

               
    }
}
