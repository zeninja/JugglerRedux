Shader "Custom/UnlitColorStencil" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_StencilRef ("Stencil Ref", float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp", int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)]       _StencilOp   ("Stencil Op",   int) = 0
	}

	SubShader {
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
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
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
				};

				fixed4 _Color;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = _Color;
					return col;
				}
			ENDCG
		}
	}
}
