Shader "Hopoo Games/FX/Cloud Remap" {
	Properties {
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendOp)] _InternalSimpleBlendMode ("Internal Simple Blend Mode", Float) = 0
		[HDR] _TintColor ("Tint", Color) = (1,1,1,1)
		[Toggle(DISABLEREMAP)] _DisableRemapOn ("Disable Remapping", Float) = 0
		_MainTex ("Base (RGB) Trans (A)", 2D) = "grey" {}
		_RemapTex ("Color Remap Ramp (RGB)", 2D) = "grey" {}
		[Toggle(SOFTPARTICLES_ON)] _SoftparticlesOn ("Enable Soft Particles", Float) = 0
		_InvFade ("Soft Factor", Range(0, 2)) = 0.1
		_Boost ("Brightness Boost", Range(1, 20)) = 1
		_AlphaBoost ("Alpha Boost", Range(0, 20)) = 1
		_AlphaBias ("Alpha Bias", Range(0, 1)) = 0
		[Toggle(USE_UV1)] _UseUV1On ("Use UV1", Float) = 0
		[Toggle(FADECLOSE)] _FadeCloseOn ("Fade when near camera needs SOFTPARTICLES_ON", Float) = 0
		_FadeCloseDistance ("Fade Close Distance", Range(0, 1)) = 0.5
		[MaterialEnum(None,0,Front,1,Back,2)] _Cull ("Culling Mode", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		_DepthOffset ("_DepthOffset", Range(-10, 10)) = 0
		[Toggle(USE_CLOUDS)] _CloudsOn ("Cloud Remapping", Float) = 1
		[Toggle(CLOUDOFFSET)] _CloudOffsetOn ("Distortion Clouds", Float) = 0
		_DistortionStrength ("Distortion Strength", Range(-2, 2)) = 0.1
		_Cloud1Tex ("Cloud 1 (RGB) Trans (A)", 2D) = "grey" {}
		_Cloud2Tex ("Cloud 2 (RGB) Trans (A)", 2D) = "grey" {}
		_CutoffScroll ("Cutoff Scroll Speed", Vector) = (0,0,0,0)
		[Toggle(VERTEXCOLOR)] _VertexColorOn ("Vertex Colors", Float) = 0
		[Toggle(VERTEXALPHA)] _VertexAlphaOn ("Luminance for Vertex Alpha", Float) = 0
		[Toggle(CALCTEXTUREALPHA)] _CalcTextureAlphaOn ("Luminance for Texture Alpha DOES NOTHING", Float) = 0
		[Toggle(VERTEXOFFSET)] _VertexOffsetOn ("Vertex Offset", Float) = 0
		[Toggle(FRESNEL)] _FresnelOn ("Fresnel Fade", Float) = 0
		[Toggle(SKYBOX_ONLY)] _SkyboxOnly ("Skybox Only (needs Cloud Remapping On)", Float) = 0
		_FresnelPower ("Fresnel Power", Range(-20, 20)) = 0
		_OffsetAmount ("Vertex Offset Amount", Range(0, 3)) = 0
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
		[PerRendererData] _Fade ("Fade", Range(0, 1)) = 1
	}
	SubShader
	{
	LOD 0
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		BlendOp [_InternalSimpleBlendMode]
		Blend [_SrcBlend] [_DstBlend]
		ColorMask RGB
		Cull [_Cull]
		Lighting Off 
		ZWrite Off
		ZTest [_ZTest]
		Pass {
			CGPROGRAM
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile_particles
			#pragma multi_compile __ LIGHTPROBE_SH
			#pragma shader_feature DISABLEREMAP
			#pragma shader_feature USE_UV1
			#pragma shader_feature USE_CLOUDS
			#pragma shader_feature CLOUDOFFSET
			#pragma shader_feature SKYBOX_ONLY
			#pragma shader_feature FADECLOSE
			#pragma shader_feature VERTEXOFFSET
			#pragma shader_feature VERTEXALPHA
			#pragma shader_feature VERTEXCOLOR
			#pragma shader_feature FRESNEL
			#pragma multi_compile_fog
			#include "UnityShaderVariables.cginc"
			#include "UnityCG.cginc"
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float _SrcBlend;
			uniform float _DstBlend;
			uniform float _InternalSimpleBlendMode;
			uniform fixed4 _TintColor;
			uniform float _DisableRemapOn;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _RemapTex;
			uniform float4 _RemapTex_ST;
			uniform float _InvFade;
			uniform float _Boost;
			uniform float _AlphaBoost;
			uniform float _AlphaBias;
			uniform float _UseUV1On;
			uniform float _FadeCloseOn;
			uniform float _FadeCloseDistance;
			uniform float _Cull;
			uniform float _ZTest;
			uniform float _DepthOffset;
			uniform float _CloudsOn;
			uniform float _CloudOffsetOn;
			uniform float _DistortionStrength;
			uniform sampler2D _Cloud1Tex;
			uniform float4 _Cloud1Tex_ST;
			uniform sampler2D _Cloud2Tex;
			uniform float4 _Cloud2Tex_ST;
			uniform float4 _CutoffScroll;
			uniform float _VertexColorOn;
			uniform float _VertexAlphaOn;
			uniform float _CalcTextureAlphaOn;
			uniform float _VertexOffsetOn;
			uniform float _FresnelOn;
			uniform float _SkyboxOnly;
			uniform float _FresnelPower;
			uniform float _OffsetAmount;
			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _ExternalAlpha)
			UNITY_DEFINE_INSTANCED_PROP(float, _Fade)
			UNITY_INSTANCING_BUFFER_END(Props)

			//DEFINE 0 because the original shader had really weird shader feature conditions
			#if VERTEXCOLOR
				#define USE_UV1 0
			#endif	
			#if USE_UV1 || VERTEXOFFSET || FRESNEL || VERTEXALPHA || VERTEXOFFSET || FADECLOSE
				#define DISABLEREMAP 0
				#define SKYBOX_ONLY 0
			#endif
			#if DISABLEREMAP
				#define SKYBOX_ONLY 0
			#endif
			#if VERTEXALPHA
				#define USE_UV1 0
			#endif
			#if VERTEXALPHA || VERTEXCOLOR
				#define VERTEXOFFSET 0
			#endif
			#if VERTEXOFFSET 
				#define FADECLOSE 0
			#endif

			struct appdata_t 
			{
				float4 vertex : POSITION; //v0
				float4 tangent : TANGENT; //v1
				float3 normal : NORMAL; //v2
				float4 uv0 : TEXCOORD0; //v3
				float4 uv1 : TEXCOORD1; //v4
				float4 texcoord2 : TEXCOORD2; //v5
				float4 texcoord3 : TEXCOORD3; //v6
				fixed4 color : COLOR; //v7
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			struct v2f 
			{
				float4 posCS : SV_POSITION; //o0
				float4 uv : TEXCOORD0; //o1
				#if USE_CLOUDS
					float4 uv1 : TEXCOORD1; //o2
				#endif
				float3 normalWS : TEXCOORD2; //o2/3
				float3 posWS : TEXCOORD3; //o3/4
				float4 screenPos : TEXCOORD4; //o4/5
				fixed4 color : COLOR; //o6
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};

			#define COLUMN(mat,index) mat._m0##index##_m1##index##_m2##index##_m3##index

			v2f vert ( appdata_t v  )
			{
				//RenderForward.RenderLoopJob
				/* VS core
				r0.xyzw = cb2[1].xyzw * v0.yyyy;
				r0.xyzw = cb2[0].xyzw * v0.xxxx + r0.xyzw;
				r0.xyzw = cb2[2].xyzw * v0.zzzz + r0.xyzw;
				r1.xyzw = cb2[3].xyzw + r0.xyzw;
				o3.xyz = cb2[3].xyz * v0.www + r0.xyz;
				r0.xyzw = cb3[18].xyzw * r1.yyyy;
				r0.xyzw = cb3[17].xyzw * r1.xxxx + r0.xyzw;
				r0.xyzw = cb3[19].xyzw * r1.zzzz + r0.xyzw;
				r0.xyzw = cb3[20].xyzw * r1.wwww + r0.xyzw;
				o0.xyzw = r0.xyzw;
				o1.xy = v3.xy * cb0[11].xy + cb0[11].zw;
				r1.x = dot(v2.xyz, cb2[4].xyz);
				r1.y = dot(v2.xyz, cb2[5].xyz);
				r1.z = dot(v2.xyz, cb2[6].xyz);
				r1.w = dot(r1.xyz, r1.xyz);
				r1.w = rsqrt(r1.w);
				o2.xyz = r1.xyz * r1.www;
				r0.y = cb1[5].x * r0.y;
				r1.xzw = float3(0.5,0.5,0.5) * r0.xwy;
				o4.zw = r0.zw;
				o4.xy = r1.xw + r1.zz;
				o5.xyzw = v7.xyzw;
				o6.xyzw = float4(0,0,0,0);
				o7.xyzw = float4(0,0,0,0);
				return;
				*/
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				#if VERTEXOFFSET
					v.vertex.xyz += _OffsetAmount * normalize(v.normal.xyz);
				#endif
				//the original shader had weird ways to select between vector and point WS
				float4 vectorWS = COLUMN(unity_ObjectToWorld, 0) * v.vertex.x +
					COLUMN(unity_ObjectToWorld, 1) * v.vertex.y +
					COLUMN(unity_ObjectToWorld, 2) * v.vertex.z;
				float4 pointWS = vectorWS + COLUMN(unity_ObjectToWorld, 3); //with translation
				o.posWS = vectorWS.xyz + unity_ObjectToWorld[3].xyz * v.vertex.w; //select WS
				o.posCS = mul(UNITY_MATRIX_VP, pointWS); //always uses pointWS
				float2 uvSelected = v.uv0.xy;
				#if USE_UV1
					uvSelected = v.uv1.xy;
				#endif
				o.uv.xy = TRANSFORM_TEX(uvSelected, _MainTex);
				#if USE_CLOUDS
					o.uv.zw = TRANSFORM_TEX(uvSelected, _Cloud1Tex);
					o.uv1.xy = TRANSFORM_TEX(uvSelected, _Cloud2Tex);
				#endif
				o.normalWS = UnityObjectToWorldNormal(v.normal.xyz);
				o.screenPos = ComputeScreenPos(o.posCS); //_DisableRemapOn removes this but whatever
				o.color = v.color;
				UNITY_TRANSFER_FOG(o, o.posCS);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				/* PS core
				r0.xyzw = t0.Sample(s0_s, v1.xy).xyzw;
				r0.x = cb0[8].w * r0.w;
				r0.x = v4.w * r0.x;
				r0.x = r0.x * cb0[10].x + cb0[10].y;
				r0.x = cb0[9].x * r0.x;
				r0.x = cb0[9].z * r0.x;
				r0.y = 0.5;
				r0.xyzw = t1.Sample(s1_s, r0.xy).xyzw;
				r0.xyzw = cb0[8].xyzw * r0.xyzw;
				o0.xyz = cb0[9].www * r0.xyz;
				o0.w = r0.w;
				*/
				UNITY_SETUP_INSTANCE_ID( i );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

				float4 finalColor;
				float fresnel = 1.0;
				float sceneZ = 1.0;
				#if (SKYBOX_ONLY && USE_CLOUDS) || SOFTPARTICLES_ON
					sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
				#endif
				#if SKYBOX_ONLY && USE_CLOUDS
					if (Linear01Depth(sceneZ) - 0.999899983 < 0.0)
					{
						discard;
					}
				#endif
				#if FRESNEL
					float3 vertexToCamera = normalize(_WorldSpaceCameraPos.xyz - i.posWS);
					float fresnelAlignment = dot(vertexToCamera, i.normalWS);
					fresnel = float(0.0 < _FresnelPower) - float(_FresnelPower < 0.0);
					fresnel = saturate(saturate(fresnel) - fresnel * fresnelAlignment);
					fresnel = exp2(abs(_FresnelPower) * log2(fresnel));
				#endif
				float4 mainTexColor = tex2D(_MainTex, i.uv.xy);
				#if DISABLEREMAP
					finalColor = mainTexColor * _TintColor * i.color;
					finalColor.a = finalColor.a * _AlphaBoost + _AlphaBias; 
				#else
					float softFadeOut = 1.0;
					float fadeNear = 1.0;
					#if SOFTPARTICLES_ON
						float partZ = i.screenPos.w + _DepthOffset;
						float fade = saturate (_InvFade * (LinearEyeDepth(sceneZ) - partZ));
						bool invFadeOver0 = 0.0 < _InvFade;
						softFadeOut = invFadeOver0 ? fade : 1.0;
						#if FADECLOSE
							fadeNear = saturate(_FadeCloseDistance * (partZ - 0.5));
							fadeNear = invFadeOver0 ? fadeNear : 1.0;
						#endif
					#endif
					float2 uv = float2(1.0, 0.5);
					#if USE_CLOUDS
						float cloudScroll = _Time.x + i.color.r + i.color.g + i.color.b;
						float4 cloudUVs = float4(cloudScroll * _CutoffScroll.xy + i.uv.zw,
						 cloudScroll * _CutoffScroll.zw + i.uv1.xy);
						float2 cloudAlphas = float2(tex2D(_Cloud1Tex, cloudUVs.xy).a, tex2D(_Cloud2Tex, cloudUVs.zw).a);
						#if CLOUDOFFSET
							float2 cloudOffset = i.uv.xy;
							cloudOffset.y += _DistortionStrength * (dot(cloudAlphas.x, cloudAlphas.y) * 2.0 - 1.0);
							uv.x *= tex2D(_MainTex, cloudOffset).a * _TintColor.a;
						#else
							uv.x *= 4.0 * cloudAlphas.x * cloudAlphas.y;
						#endif
					#endif
					uv.x = uv.x * mainTexColor.a * _TintColor.a * i.color.a * _AlphaBoost + _AlphaBias;
					uv.x *= UNITY_ACCESS_INSTANCED_PROP(Props, _ExternalAlpha) *
					 UNITY_ACCESS_INSTANCED_PROP(Props, _Fade) * softFadeOut * fadeNear *
					 (_VertexAlphaOn > 0.5 ? i.color.r : 1.0) * fresnel;
					finalColor = tex2D(_RemapTex, uv) * _TintColor;
				#endif
				finalColor.rgb *= (_VertexColorOn > 0.5 ? i.color.rgb : 1.0);
				finalColor.rgb *= _Boost;
				UNITY_APPLY_FOG(i.fogCoord, finalColor);
				return finalColor;
			}
			ENDCG 
		}
	}	
	Fallback "Transparent/VertexLit"
}