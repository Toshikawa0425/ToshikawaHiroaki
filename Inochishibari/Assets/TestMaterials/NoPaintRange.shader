// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NoPaintRange" {
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "black" {}
    }

		SubShader
    {
        Tags {"Queue" = "Geometry+202" }
        Pass{
        Stencil
        {
            Ref 1
            Comp Equal
            Pass IncrSat
    }
            ColorMask 0
            ZTest Always //ê[ìxÇ…ä÷åWÇ»Ç≠
            ZWrite Off
    }
        Pass{
            Stencil
        {
            Ref 2
            Comp Equal
    }
        ZTest Always
            ZWrite Off
            ColorMask 0
}

            
        
    }
        Fallback "Diffuse"
}
			

