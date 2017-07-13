Shader "Custom/Window" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _StencilVal ("stencilVal", Int) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        ZWrite Off
         ColorMask 0
   
         Pass {
             Stencil {
                 Ref [_StencilVal]
                 Comp NotEqual //always
                 Pass replace
             }
         }
 
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}