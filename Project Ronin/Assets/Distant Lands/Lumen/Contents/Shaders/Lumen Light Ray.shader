// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Lumen/Light Ray"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][PerRendererData]_CameraDistanceFadeStart("Camera Distance Fade Start", Float) = 20
		[PerRendererData]_CameraDistanceFadeEnd("Camera Distance Fade End", Float) = 30
		[PerRendererData][Toggle][Enum(Static,0,Dynamic,1)]_Style("Style", Float) = 0
		[HDR][PerRendererData]_MainColor("Main Color", Color) = (1,1,1,0.454902)
		[PerRendererData]_Intensity("Intensity", Range( 0 , 1)) = 1
		[PerRendererData][Toggle]_Bidirectional("Bidirectional", Float) = 1
		[PerRendererData]_AngleOpacityEffect("Angle Opacity Effect", Range( 0 , 1)) = 1
		[PerRendererData]_AngleRaylengthEffect("Angle Raylength Effect", Range( 0 , 1)) = 1
		[PerRendererData]_RayLength("Ray Length", Float) = 10
		[PerRendererData][Toggle]_AutoAssignSun("Auto Assign Sun", Float) = 0
		[PerRendererData][Toggle]_UseLumenSunScript("Use Lumen Sun Script", Float) = 0
		[PerRendererData]_SunDirection("Sun Direction", Vector) = (0,-1,0,0)
		[PerRendererData][Toggle]_UseCameraDepthFade("Use Camera Depth Fade", Float) = 1
		[PerRendererData][Toggle]_UseCameraDistanceFade("Use Camera Distance Fade", Float) = 1
		[PerRendererData]_CameraDepthFadeStart("Camera Depth Fade Start", Float) = 0
		[PerRendererData]_CameraDepthFadeEnd("Camera Depth Fade End", Float) = 2
		[PerRendererData][Toggle]_UseSceneDepthFade("Use Scene Depth Fade", Float) = 1
		[PerRendererData][Toggle]_UseLightColor("Use Light Color", Float) = 0
		[PerRendererData]_DepthFadeStartDistance("Depth Fade Start Distance", Float) = 0
		[PerRendererData]_DepthFadeEndDistance("Depth Fade End Distance", Float) = 2
		[PerRendererData][Toggle]_UseAngleBasedFade("Use Angle Based Fade", Float) = 0
		[PerRendererData]_AngleFade("Angle Fade", Float) = 1
		[PerRendererData]_AngleFadeStart("Angle Fade Start", Float) = 0.5
		[PerRendererData][Toggle]_UseVariation("Use Variation", Float) = 1
		[PerRendererData]_VariationSpeed("Variation Speed", Range( 0 , 10)) = 1
		[PerRendererData]_VariationScale("Variation Scale", Float) = 1
		[perRendererData][Toggle]_UseUniformVariation("Use Uniform Variation", Float) = 0
		[ASEEnd][HDR][PerRendererData]_VariationColor("Variation Color", Color) = (1,1,1,0.454902)

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "LightMode"="UniversalForwardOnly" }
		
		Cull Off
		AlphaToMask Off
		
		HLSLINCLUDE
		#pragma target 2.0

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest Always
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 100600
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _VariationColor;
			float3 _SunDirection;
			float _Style;
			float _DepthFadeEndDistance;
			float _DepthFadeStartDistance;
			float _UseSceneDepthFade;
			float _CameraDistanceFadeStart;
			float _CameraDistanceFadeEnd;
			float _UseCameraDistanceFade;
			float _CameraDepthFadeEnd;
			float _CameraDepthFadeStart;
			float _UseCameraDepthFade;
			float _AngleFade;
			float _AngleFadeStart;
			float _VariationScale;
			float _AngleOpacityEffect;
			float _VariationSpeed;
			float _UseUniformVariation;
			float _UseVariation;
			float _UseLightColor;
			float _Bidirectional;
			float _AngleRaylengthEffect;
			float _RayLength;
			float _UseLumenSunScript;
			float _AutoAssignSun;
			float _UseAngleBasedFade;
			float _Intensity;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float3 LUMEN_SunDir;
			uniform float4 _CameraDepthTexture_TexelSize;


			real3 ASESafeNormalize(float3 inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float Style144 = _Style;
				float3 normalizeResult28_g111 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( _MainLightPosition.xyz )) ):( _SunDirection )) );
				float3 SunDir31_g111 = -normalizeResult28_g111;
				float4 transform3_g111 = mul(GetObjectToWorldMatrix(),float4( float3(0,0,1) , 0.0 ));
				float dotResult5_g111 = dot( transform3_g111 , float4( -normalizeResult28_g111 , 0.0 ) );
				float lerpResult36_g111 = lerp( ( 1.0 - _AngleRaylengthEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g111 ) ):( dotResult5_g111 )));
				float VertexAngle11_g111 = saturate( lerpResult36_g111 );
				float3 worldToObjDir20_g111 = mul( GetWorldToObjectMatrix(), float4( ( ( 1.0 - v.ase_color.g ) * SunDir31_g111 * _RayLength * VertexAngle11_g111 ), 0 ) ).xyz;
				float3 DynamicRayVector123 = worldToObjDir20_g111;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( Style144 == 1.0 ? DynamicRayVector123 : float3( 0,0,0 ) );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				float2 appendResult22_g98 = (float2(( WorldPosition.x + WorldPosition.y ) , WorldPosition.z));
				float4 transform26_g98 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float2 appendResult28_g98 = (float2(( transform26_g98.x + transform26_g98.y ) , transform26_g98.z));
				float2 UVPos31_g98 = (( _UseUniformVariation )?( appendResult28_g98 ):( appendResult22_g98 ));
				float mulTime6_g98 = _TimeParameters.x * _VariationSpeed;
				float simplePerlin2D18_g98 = snoise( (UVPos31_g98*1.0 + ( mulTime6_g98 * -0.5 ))*( 2.0 / _VariationScale ) );
				simplePerlin2D18_g98 = simplePerlin2D18_g98*0.5 + 0.5;
				float simplePerlin2D19_g98 = snoise( (UVPos31_g98*1.0 + mulTime6_g98)*( 1.0 / _VariationScale ) );
				simplePerlin2D19_g98 = simplePerlin2D19_g98*0.5 + 0.5;
				float4 lerpResult49 = lerp( _MainColor , _VariationColor , saturate( min( simplePerlin2D18_g98 , simplePerlin2D19_g98 ) ));
				float4 break137 = _MainLightColor;
				float4 appendResult138 = (float4(break137.r , break137.g , break137.b , 1.0));
				float4 RayColor99 = (( _UseLightColor )?( ( (( _UseVariation )?( lerpResult49 ):( _MainColor )) * saturate( appendResult138 ) ) ):( (( _UseVariation )?( lerpResult49 ):( _MainColor )) ));
				
				float4 transform4_g109 = mul(GetObjectToWorldMatrix(),float4( IN.ase_normal , 0.0 ));
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float dotResult6_g109 = dot( transform4_g109 , float4( ase_worldViewDir , 0.0 ) );
				float4 transform9_g110 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float4 transform9_g108 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float isOrtho154 = unity_OrthoParams.w;
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth4_g106 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth4_g106 = ( screenDepth4_g106 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 );
				float screenDepth17_g107 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth17_g107 = abs( ( screenDepth17_g107 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 ) );
				float depthToLinear18_g107 = Linear01Depth(distanceDepth17_g107,_ZBufferParams);
				float Style144 = _Style;
				float4 transform3_g111 = mul(GetObjectToWorldMatrix(),float4( float3(0,0,1) , 0.0 ));
				float3 normalizeResult28_g111 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( _MainLightPosition.xyz )) ):( _SunDirection )) );
				float dotResult5_g111 = dot( transform3_g111 , float4( -normalizeResult28_g111 , 0.0 ) );
				float lerpResult35_g111 = lerp( ( 1.0 - _AngleOpacityEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g111 ) ):( dotResult5_g111 )));
				float DynamicRayMagnitude128 = saturate( lerpResult35_g111 );
				float luminance207 = Luminance(RayColor99.rgb);
				float RayPreAlpha102 = ( ( luminance207 * RayColor99.a * IN.ase_color.r ) * _Intensity );
				float FinalAlpha112 = saturate( ( (( _UseAngleBasedFade )?( saturate( (0.0 + (abs( dotResult6_g109 ) - _AngleFadeStart) * (1.0 - 0.0) / (_AngleFade - _AngleFadeStart)) ) ):( 1.0 )) * (( _UseCameraDepthFade )?( saturate( (0.0 + (distance( transform9_g110 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDepthFadeStart) * (1.0 - 0.0) / (_CameraDepthFadeEnd - _CameraDepthFadeStart)) ) ):( 1.0 )) * (( _UseCameraDistanceFade )?( saturate( (0.0 + (distance( transform9_g108 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDistanceFadeEnd) * (1.0 - 0.0) / (_CameraDistanceFadeStart - _CameraDistanceFadeEnd)) ) ):( 1.0 )) * (( _UseSceneDepthFade )?( ( isOrtho154 == 0.0 ? saturate( (0.0 + (distanceDepth4_g106 - _DepthFadeStartDistance) * (1.0 - 0.0) / (_DepthFadeEndDistance - _DepthFadeStartDistance)) ) : saturate( ( 1.0 - depthToLinear18_g107 ) ) ) ):( 1.0 )) * ( Style144 == 1.0 ? DynamicRayMagnitude128 : 1.0 ) * RayPreAlpha102 ) );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = RayColor99.rgb;
				float Alpha = ( FinalAlpha112 * 0.6 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 100600
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float3 ase_normal : NORMAL;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _VariationColor;
			float3 _SunDirection;
			float _Style;
			float _DepthFadeEndDistance;
			float _DepthFadeStartDistance;
			float _UseSceneDepthFade;
			float _CameraDistanceFadeStart;
			float _CameraDistanceFadeEnd;
			float _UseCameraDistanceFade;
			float _CameraDepthFadeEnd;
			float _CameraDepthFadeStart;
			float _UseCameraDepthFade;
			float _AngleFade;
			float _AngleFadeStart;
			float _VariationScale;
			float _AngleOpacityEffect;
			float _VariationSpeed;
			float _UseUniformVariation;
			float _UseVariation;
			float _UseLightColor;
			float _Bidirectional;
			float _AngleRaylengthEffect;
			float _RayLength;
			float _UseLumenSunScript;
			float _AutoAssignSun;
			float _UseAngleBasedFade;
			float _Intensity;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float3 LUMEN_SunDir;
			uniform float4 _CameraDepthTexture_TexelSize;


			real3 ASESafeNormalize(float3 inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float Style144 = _Style;
				float3 normalizeResult28_g111 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( _MainLightPosition.xyz )) ):( _SunDirection )) );
				float3 SunDir31_g111 = -normalizeResult28_g111;
				float4 transform3_g111 = mul(GetObjectToWorldMatrix(),float4( float3(0,0,1) , 0.0 ));
				float dotResult5_g111 = dot( transform3_g111 , float4( -normalizeResult28_g111 , 0.0 ) );
				float lerpResult36_g111 = lerp( ( 1.0 - _AngleRaylengthEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g111 ) ):( dotResult5_g111 )));
				float VertexAngle11_g111 = saturate( lerpResult36_g111 );
				float3 worldToObjDir20_g111 = mul( GetWorldToObjectMatrix(), float4( ( ( 1.0 - v.ase_color.g ) * SunDir31_g111 * _RayLength * VertexAngle11_g111 ), 0 ) ).xyz;
				float3 DynamicRayVector123 = worldToObjDir20_g111;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( Style144 == 1.0 ? DynamicRayVector123 : float3( 0,0,0 ) );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 transform4_g109 = mul(GetObjectToWorldMatrix(),float4( IN.ase_normal , 0.0 ));
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float dotResult6_g109 = dot( transform4_g109 , float4( ase_worldViewDir , 0.0 ) );
				float4 transform9_g110 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float4 transform9_g108 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float isOrtho154 = unity_OrthoParams.w;
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth4_g106 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth4_g106 = ( screenDepth4_g106 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 );
				float screenDepth17_g107 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth17_g107 = abs( ( screenDepth17_g107 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 ) );
				float depthToLinear18_g107 = Linear01Depth(distanceDepth17_g107,_ZBufferParams);
				float Style144 = _Style;
				float4 transform3_g111 = mul(GetObjectToWorldMatrix(),float4( float3(0,0,1) , 0.0 ));
				float3 normalizeResult28_g111 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( _MainLightPosition.xyz )) ):( _SunDirection )) );
				float dotResult5_g111 = dot( transform3_g111 , float4( -normalizeResult28_g111 , 0.0 ) );
				float lerpResult35_g111 = lerp( ( 1.0 - _AngleOpacityEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g111 ) ):( dotResult5_g111 )));
				float DynamicRayMagnitude128 = saturate( lerpResult35_g111 );
				float2 appendResult22_g98 = (float2(( WorldPosition.x + WorldPosition.y ) , WorldPosition.z));
				float4 transform26_g98 = mul(GetObjectToWorldMatrix(),float4( 0,0,0,1 ));
				float2 appendResult28_g98 = (float2(( transform26_g98.x + transform26_g98.y ) , transform26_g98.z));
				float2 UVPos31_g98 = (( _UseUniformVariation )?( appendResult28_g98 ):( appendResult22_g98 ));
				float mulTime6_g98 = _TimeParameters.x * _VariationSpeed;
				float simplePerlin2D18_g98 = snoise( (UVPos31_g98*1.0 + ( mulTime6_g98 * -0.5 ))*( 2.0 / _VariationScale ) );
				simplePerlin2D18_g98 = simplePerlin2D18_g98*0.5 + 0.5;
				float simplePerlin2D19_g98 = snoise( (UVPos31_g98*1.0 + mulTime6_g98)*( 1.0 / _VariationScale ) );
				simplePerlin2D19_g98 = simplePerlin2D19_g98*0.5 + 0.5;
				float4 lerpResult49 = lerp( _MainColor , _VariationColor , saturate( min( simplePerlin2D18_g98 , simplePerlin2D19_g98 ) ));
				float4 break137 = _MainLightColor;
				float4 appendResult138 = (float4(break137.r , break137.g , break137.b , 1.0));
				float4 RayColor99 = (( _UseLightColor )?( ( (( _UseVariation )?( lerpResult49 ):( _MainColor )) * saturate( appendResult138 ) ) ):( (( _UseVariation )?( lerpResult49 ):( _MainColor )) ));
				float luminance207 = Luminance(RayColor99.rgb);
				float RayPreAlpha102 = ( ( luminance207 * RayColor99.a * IN.ase_color.r ) * _Intensity );
				float FinalAlpha112 = saturate( ( (( _UseAngleBasedFade )?( saturate( (0.0 + (abs( dotResult6_g109 ) - _AngleFadeStart) * (1.0 - 0.0) / (_AngleFade - _AngleFadeStart)) ) ):( 1.0 )) * (( _UseCameraDepthFade )?( saturate( (0.0 + (distance( transform9_g110 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDepthFadeStart) * (1.0 - 0.0) / (_CameraDepthFadeEnd - _CameraDepthFadeStart)) ) ):( 1.0 )) * (( _UseCameraDistanceFade )?( saturate( (0.0 + (distance( transform9_g108 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDistanceFadeEnd) * (1.0 - 0.0) / (_CameraDistanceFadeStart - _CameraDistanceFadeEnd)) ) ):( 1.0 )) * (( _UseSceneDepthFade )?( ( isOrtho154 == 0.0 ? saturate( (0.0 + (distanceDepth4_g106 - _DepthFadeStartDistance) * (1.0 - 0.0) / (_DepthFadeEndDistance - _DepthFadeStartDistance)) ) : saturate( ( 1.0 - depthToLinear18_g107 ) ) ) ):( 1.0 )) * ( Style144 == 1.0 ? DynamicRayMagnitude128 : 1.0 ) * RayPreAlpha102 ) );
				
				float Alpha = ( FinalAlpha112 * 0.6 );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18934
0;1080;2194.286;607.5715;3120.989;534.6324;1.006732;True;False
Node;AmplifyShaderEditor.CommentaryNode;98;-3696,-352;Inherit;False;1505.227;558.2675;;11;99;208;117;120;94;138;49;48;1;137;118;Ray Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.LightColorNode;118;-3360,-32;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;1;-3616,-96;Inherit;False;Property;_VariationColor;Variation Color;33;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;1,1,1,0.1294118;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;245;-3552,80;Inherit;False;Variation;29;;98;487039bf64ba820499085da4bb1fd9c4;1,34,1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-3616,-272;Inherit;False;Property;_MainColor;Main Color;4;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;1,1,1,0.454902;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;137;-3216,-32;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;49;-3376,-160;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;138;-3088,-32;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;94;-3216,-240;Inherit;False;Property;_UseVariation;Use Variation;28;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;120;-2960,-32;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-2800,-112;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;208;-2640,-240;Inherit;False;Property;_UseLightColor;Use Light Color;20;0;Create;True;0;0;0;False;1;PerRendererData;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-2416,-240;Inherit;False;RayColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;97;-3696,288;Inherit;False;1220.257;534.7493;;8;102;18;3;63;2;50;207;101;Pre-Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;152;-2128,-352;Inherit;False;536.7587;547.5398;;6;154;153;123;144;143;128;Variables;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-3664,336;Inherit;False;99;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;2;-3472,592;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OrthoParams;153;-2080,32;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;50;-3424,448;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LuminanceNode;207;-3424,352;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-1856,32;Inherit;False;isOrtho;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-2096,-288;Inherit;False;Property;_Style;Style;3;3;[PerRendererData];[Toggle];[Enum];Create;True;0;2;Static;0;Dynamic;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;253;-2096,-192;Inherit;False;Dynamic Raylength;6;;111;7ea6d9a6da4456540a55b4eec4ab7725;0;0;2;FLOAT3;0;FLOAT;21
Node;AmplifyShaderEditor.CommentaryNode;114;-2432,288;Inherit;False;1412.842;843.2496;;17;112;119;111;110;95;103;108;147;161;148;125;209;204;213;234;215;217;Fading Functions;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3184,576;Inherit;False;Property;_Intensity;Intensity;5;1;[PerRendererData];Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-3184,448;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;209;-2352,800;Inherit;False;Orthographic Fade;-1;;107;07ff034ed2c5b5b4783cf486c6ebd7b0;0;0;1;FLOAT;6
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2848,528;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;251;-2352,720;Inherit;False;Scene Depth Fade;21;;106;b80e7cce07f3ff241b5ed9e75228c4d5;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-1872,-96;Inherit;False;DynamicRayMagnitude;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-1872,-288;Inherit;False;Style;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;204;-2352,624;Inherit;False;154;isOrtho;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2112,976;Inherit;False;128;DynamicRayMagnitude;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;102;-2688,528;Inherit;False;RayPreAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;215;-2128,592;Inherit;False;Camera Distance Fade;0;;108;f992419659205874689e17721bd92798;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;161;-2112,704;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;234;-2128,368;Inherit;False;Angle Based Fade;25;;109;f37da3baba36450419915a0eb03bfcd4;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;217;-2128,480;Inherit;False;Camera Depth Fade;16;;110;b003bbeb8eacda14fa920f9527150e3e;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;148;-2112,880;Inherit;False;144;Style;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-1840,1024;Inherit;False;102;RayPreAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;147;-1824,864;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;95;-1904,368;Inherit;False;Property;_UseAngleBasedFade;Use Angle Based Fade;24;0;Create;True;0;0;0;False;1;PerRendererData;False;0;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;213;-1904,592;Inherit;False;Property;_UseCameraDistanceFade;Use Camera Distance Fade;15;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;103;-1904,480;Inherit;False;Property;_UseCameraDepthFade;Use Camera Depth Fade;14;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;108;-1888,736;Inherit;False;Property;_UseSceneDepthFade;Use Scene Depth Fade;19;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-1552,512;Inherit;False;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;119;-1408,512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;-1856,-192;Inherit;False;DynamicRayVector;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-1264,512;Inherit;False;FinalAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-386,36;Inherit;False;112;FinalAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-512,256;Inherit;False;123;DynamicRayVector;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;-464,176;Inherit;False;144;Style;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;145;-257,176;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-366,-51;Inherit;False;99;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;-162.7349,56.09056;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;246;-33,-86;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;248;-33,-86;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;249;-33,-86;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;250;-33,-86;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;247;130,39;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Distant Lands/Lumen/Light Ray;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;LightMode=UniversalForwardOnly;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;0;False;-1;True;7;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;637866970490943587;  Blend;0;0;Two Sided;0;637866970474173615;Cast Shadows;0;637866970522117174;  Use Shadow Threshold;0;0;Receive Shadows;0;637866970529056200;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;True;False;False;;False;0
WireConnection;137;0;118;0
WireConnection;49;0;48;0
WireConnection;49;1;1;0
WireConnection;49;2;245;0
WireConnection;138;0;137;0
WireConnection;138;1;137;1
WireConnection;138;2;137;2
WireConnection;94;0;48;0
WireConnection;94;1;49;0
WireConnection;120;0;138;0
WireConnection;117;0;94;0
WireConnection;117;1;120;0
WireConnection;208;0;94;0
WireConnection;208;1;117;0
WireConnection;99;0;208;0
WireConnection;50;0;101;0
WireConnection;207;0;101;0
WireConnection;154;0;153;4
WireConnection;3;0;207;0
WireConnection;3;1;50;3
WireConnection;3;2;2;1
WireConnection;18;0;3;0
WireConnection;18;1;63;0
WireConnection;128;0;253;21
WireConnection;144;0;143;0
WireConnection;102;0;18;0
WireConnection;161;0;204;0
WireConnection;161;2;251;0
WireConnection;161;3;209;6
WireConnection;147;0;148;0
WireConnection;147;2;125;0
WireConnection;95;1;234;0
WireConnection;213;1;215;0
WireConnection;103;1;217;0
WireConnection;108;1;161;0
WireConnection;111;0;95;0
WireConnection;111;1;103;0
WireConnection;111;2;213;0
WireConnection;111;3;108;0
WireConnection;111;4;147;0
WireConnection;111;5;110;0
WireConnection;119;0;111;0
WireConnection;123;0;253;0
WireConnection;112;0;119;0
WireConnection;145;0;146;0
WireConnection;145;2;124;0
WireConnection;252;0;113;0
WireConnection;247;2;100;0
WireConnection;247;3;252;0
WireConnection;247;5;145;0
ASEEND*/
//CHKSM=79487D5044686DD7714133DB5E47715A14DEDBA6