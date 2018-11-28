// Shader "Custom/d_LineMask(SIMPLE)" {
//     Properties {
//         _StencilRef ("Stencil Ref", float) = 0
//         [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp", int) = 0
//         [Enum(UnityEngine.Rendering.StencilOp)]       _StencilOp   ("Stencil Op",   int) = 0
//     }

//     SubShader {
//         Tags { "RenderType"="Opaque" "Queue"="Background-1"}
//         Pass {
// 			Stencil {
// 				Ref  [_StencilRef]
// 				Comp [_StencilComp]
// 				Pass [_StencilOp]
//                 Fail Zero
// 			}

//             Zwrite Off
//             Blend SrcAlpha OneMinusSrcAlpha

        
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             struct appdata {
//                 float4 vertex : POSITION;
//             };
//             struct v2f {
//                 float4 pos : SV_POSITION;
//             };
//             v2f vert(appdata v) {
//                 v2f o;
//                 o.pos = UnityObjectToClipPos(v.vertex);
//                 return o;
//             }
//             half4 frag(v2f i) : SV_Target {
//                 return half4(0,0,0,0);
//             }
//             ENDCG
//         }
//     } 
// }
















// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/d_LineMask" {
Properties {
    [PerRendererData]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    // [PerRendererData]_MainTex ("Particle Texture", 2D) = "white" {}
    // [PerRendererData]_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    [PerRendererData]_StencilRef ("Stencil Ref", float) = 0
    [PerRendererData][Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp", int) = 0
    [PerRendererData][Enum(UnityEngine.Rendering.StencilOp)]       _StencilOp   ("Stencil Op",   int) = 0
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask RGB
    Cull Off Lighting Off 
    //ZWrite Off

    SubShader {
        Pass {

			Stencil {
				Ref  [_StencilRef]
				Comp [_StencilComp]
				Pass [_StencilOp]
                Fail Zero
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color * _TintColor;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
                #endif

                fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
                col.a = saturate(col.a); // alpha should not have double-brightness applied to it, but we can't fix that legacy behaior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
}
