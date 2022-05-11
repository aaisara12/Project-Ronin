// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Lumen/Light Ray"
{
	Properties
	{
		[PerRendererData]_CameraDistanceFadeStart("Camera Distance Fade Start", Float) = 20
		[PerRendererData]_CameraDistanceFadeEnd("Camera Distance Fade End", Float) = 30
		[PerRendererData][Toggle][Enum(Static,0,Dynamic,1)]_Style("Style", Float) = 0
		[HDR][PerRendererData]_MainColor("Main Color", Color) = (1,1,1,0.454902)
		[PerRendererData]_Intensity("Intensity", Range( 0 , 1)) = 1
		[PerRendererData][Toggle]_Bidirectional("Bidirectional", Float) = 1
		[PerRendererData]_AngleOpacityEffect("Angle Opacity Effect", Range( 0 , 1)) = 1
		[PerRendererData]_AngleRaylengthEffect("Angle Raylength Effect", Range( 0 , 1)) = 1
		[PerRendererData]_RayLength("Ray Length", Float) = 10
		[PerRendererData]_SunDirection("Sun Direction", Vector) = (0,-1,0,0)
		[PerRendererData][Toggle]_UseLumenSunScript("UseLumenSunScript", Float) = 0
		[PerRendererData][Toggle]_AutoAssignSun("Auto Assign Sun", Float) = 0
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
		[HDR][PerRendererData]_VariationColor("Variation Color", Color) = (1,1,1,0.454902)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZTest Always
		Stencil
		{
			Ref 153
		}
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
			float4 vertexColor : COLOR;
		};

		uniform float _Style;
		uniform float _AutoAssignSun;
		uniform float3 _SunDirection;
		uniform float _UseLumenSunScript;
		uniform float3 LUMEN_SunDir;
		uniform float _RayLength;
		uniform float _AngleRaylengthEffect;
		uniform float _Bidirectional;
		uniform float _UseLightColor;
		uniform float _UseVariation;
		uniform float4 _MainColor;
		uniform float4 _VariationColor;
		uniform float _UseUniformVariation;
		uniform float _VariationSpeed;
		uniform float _VariationScale;
		uniform float _UseAngleBasedFade;
		uniform float _AngleFadeStart;
		uniform float _AngleFade;
		uniform float _UseCameraDepthFade;
		uniform float _CameraDepthFadeStart;
		uniform float _CameraDepthFadeEnd;
		uniform float _UseCameraDistanceFade;
		uniform float _CameraDistanceFadeEnd;
		uniform float _CameraDistanceFadeStart;
		uniform float _UseSceneDepthFade;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeStartDistance;
		uniform float _DepthFadeEndDistance;
		uniform float _AngleOpacityEffect;
		uniform float _Intensity;


		inline float3 ASESafeNormalize(float3 inVec)
		{
			float dp3 = max( 0.001f , dot( inVec , inVec ) );
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float Style144 = _Style;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult28_g113 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( ase_worldlightDir )) ):( _SunDirection )) );
			float3 SunDir31_g113 = -normalizeResult28_g113;
			float4 transform3_g113 = mul(unity_ObjectToWorld,float4( float3(0,0,1) , 0.0 ));
			float dotResult5_g113 = dot( transform3_g113 , float4( -normalizeResult28_g113 , 0.0 ) );
			float lerpResult36_g113 = lerp( ( 1.0 - _AngleRaylengthEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g113 ) ):( dotResult5_g113 )));
			float VertexAngle11_g113 = saturate( lerpResult36_g113 );
			float3 worldToObjDir20_g113 = mul( unity_WorldToObject, float4( ( ( 1.0 - v.color.g ) * SunDir31_g113 * _RayLength * VertexAngle11_g113 ), 0 ) ).xyz;
			float3 DynamicRayVector123 = worldToObjDir20_g113;
			v.vertex.xyz += ( Style144 == 1.0 ? DynamicRayVector123 : float3( 0,0,0 ) );
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult22_g98 = (float2(( ase_worldPos.x + ase_worldPos.y ) , ase_worldPos.z));
			float4 transform26_g98 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float2 appendResult28_g98 = (float2(( transform26_g98.x + transform26_g98.y ) , transform26_g98.z));
			float2 UVPos31_g98 = (( _UseUniformVariation )?( appendResult28_g98 ):( appendResult22_g98 ));
			float mulTime6_g98 = _Time.y * _VariationSpeed;
			float simplePerlin2D18_g98 = snoise( (UVPos31_g98*1.0 + ( mulTime6_g98 * -0.5 ))*( 2.0 / _VariationScale ) );
			simplePerlin2D18_g98 = simplePerlin2D18_g98*0.5 + 0.5;
			float simplePerlin2D19_g98 = snoise( (UVPos31_g98*1.0 + mulTime6_g98)*( 1.0 / _VariationScale ) );
			simplePerlin2D19_g98 = simplePerlin2D19_g98*0.5 + 0.5;
			float4 lerpResult49 = lerp( _MainColor , _VariationColor , saturate( min( simplePerlin2D18_g98 , simplePerlin2D19_g98 ) ));
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 break137 = ase_lightColor;
			float4 appendResult138 = (float4(break137.r , break137.g , break137.b , 1.0));
			float4 RayColor99 = (( _UseLightColor )?( ( (( _UseVariation )?( lerpResult49 ):( _MainColor )) * saturate( appendResult138 ) ) ):( (( _UseVariation )?( lerpResult49 ):( _MainColor )) ));
			o.Emission = RayColor99.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			ase_vertexNormal = normalize( ase_vertexNormal );
			float4 transform4_g111 = mul(unity_ObjectToWorld,float4( ase_vertexNormal , 0.0 ));
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult6_g111 = dot( transform4_g111 , float4( ase_worldViewDir , 0.0 ) );
			float4 transform9_g112 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float4 transform9_g110 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float isOrtho154 = unity_OrthoParams.w;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth4_g109 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth4_g109 = ( screenDepth4_g109 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 );
			float screenDepth17_g108 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth17_g108 = abs( ( screenDepth17_g108 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float depthToLinear18_g108 = Linear01Depth(distanceDepth17_g108);
			float Style144 = _Style;
			float4 transform3_g113 = mul(unity_ObjectToWorld,float4( float3(0,0,1) , 0.0 ));
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult28_g113 = ASESafeNormalize( (( _AutoAssignSun )?( (( _UseLumenSunScript )?( LUMEN_SunDir ):( ase_worldlightDir )) ):( _SunDirection )) );
			float dotResult5_g113 = dot( transform3_g113 , float4( -normalizeResult28_g113 , 0.0 ) );
			float lerpResult35_g113 = lerp( ( 1.0 - _AngleOpacityEffect ) , 1.0 , (( _Bidirectional )?( abs( dotResult5_g113 ) ):( dotResult5_g113 )));
			float DynamicRayMagnitude128 = saturate( lerpResult35_g113 );
			float luminance207 = Luminance(RayColor99.rgb);
			float RayPreAlpha102 = ( ( luminance207 * RayColor99.a * i.vertexColor.r ) * _Intensity );
			float FinalAlpha112 = saturate( ( (( _UseAngleBasedFade )?( saturate( (0.0 + (abs( dotResult6_g111 ) - _AngleFadeStart) * (1.0 - 0.0) / (_AngleFade - _AngleFadeStart)) ) ):( 1.0 )) * (( _UseCameraDepthFade )?( saturate( (0.0 + (distance( transform9_g112 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDepthFadeStart) * (1.0 - 0.0) / (_CameraDepthFadeEnd - _CameraDepthFadeStart)) ) ):( 1.0 )) * (( _UseCameraDistanceFade )?( saturate( (0.0 + (distance( transform9_g110 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDistanceFadeEnd) * (1.0 - 0.0) / (_CameraDistanceFadeStart - _CameraDistanceFadeEnd)) ) ):( 1.0 )) * (( _UseSceneDepthFade )?( ( isOrtho154 == 0.0 ? saturate( (0.0 + (distanceDepth4_g109 - _DepthFadeStartDistance) * (1.0 - 0.0) / (_DepthFadeEndDistance - _DepthFadeStartDistance)) ) : saturate( ( 1.0 - depthToLinear18_g108 ) ) ) ):( 1.0 )) * ( Style144 == 1.0 ? DynamicRayMagnitude128 : 1.0 ) * RayPreAlpha102 ) );
			o.Alpha = FinalAlpha112;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=18935
0;1080;2194;606;2975.866;380.2675;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;98;-3696,-352;Inherit;False;1505.227;558.2675;;11;99;208;117;120;94;138;49;48;1;137;118;Ray Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.LightColorNode;118;-3360,-32;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;1;-3616,-96;Inherit;False;Property;_VariationColor;Variation Color;34;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;1,1,1,0.1294118;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;245;-3552,80;Inherit;False;Variation;30;;98;487039bf64ba820499085da4bb1fd9c4;1,34,1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-3616,-272;Inherit;False;Property;_MainColor;Main Color;5;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;1,1,1,0.454902;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;137;-3216,-32;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;49;-3376,-160;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;138;-3088,-32;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;94;-3216,-240;Inherit;False;Property;_UseVariation;Use Variation;29;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;120;-2960,-32;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-2800,-112;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;208;-2640,-240;Inherit;False;Property;_UseLightColor;Use Light Color;21;0;Create;True;0;0;0;False;1;PerRendererData;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-2416,-240;Inherit;False;RayColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;97;-3696,288;Inherit;False;1220.257;534.7493;;8;102;18;3;63;2;50;207;101;Pre-Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-3664,336;Inherit;False;99;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;152;-2128,-352;Inherit;False;536.7587;547.5398;;7;154;153;123;144;143;128;267;Variables;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;2;-3472,592;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OrthoParams;153;-2080,32;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;50;-3424,448;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LuminanceNode;207;-3424,352;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-3184,448;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3184,576;Inherit;False;Property;_Intensity;Intensity;6;1;[PerRendererData];Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;267;-2096,-192;Inherit;False;Dynamic Raylength;7;;113;7ea6d9a6da4456540a55b4eec4ab7725;0;0;2;FLOAT3;0;FLOAT;21
Node;AmplifyShaderEditor.RangedFloatNode;143;-2096,-288;Inherit;False;Property;_Style;Style;4;3;[PerRendererData];[Toggle];[Enum];Create;True;0;2;Static;0;Dynamic;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-1856,32;Inherit;False;isOrtho;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;114;-2432,288;Inherit;False;1412.842;843.2496;;18;112;119;111;110;95;103;108;147;161;148;163;125;209;204;213;234;215;217;Fading Functions;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-1872,-288;Inherit;False;Style;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;163;-2352,720;Inherit;False;Scene Depth Fade;22;;109;b80e7cce07f3ff241b5ed9e75228c4d5;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;209;-2352,800;Inherit;False;Orthographic Fade;-1;;108;07ff034ed2c5b5b4783cf486c6ebd7b0;0;0;1;FLOAT;6
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-1872,-96;Inherit;False;DynamicRayMagnitude;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2848,528;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;204;-2352,624;Inherit;False;154;isOrtho;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;102;-2688,528;Inherit;False;RayPreAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;215;-2128,592;Inherit;False;Camera Distance Fade;1;;110;f992419659205874689e17721bd92798;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;161;-2112,704;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;234;-2128,368;Inherit;False;Angle Based Fade;26;;111;f37da3baba36450419915a0eb03bfcd4;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;217;-2128,480;Inherit;False;Camera Depth Fade;17;;112;b003bbeb8eacda14fa920f9527150e3e;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;148;-2112,880;Inherit;False;144;Style;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2112,976;Inherit;False;128;DynamicRayMagnitude;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;103;-1904,480;Inherit;False;Property;_UseCameraDepthFade;Use Camera Depth Fade;15;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-1840,1024;Inherit;False;102;RayPreAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;108;-1888,736;Inherit;False;Property;_UseSceneDepthFade;Use Scene Depth Fade;20;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;147;-1824,864;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;95;-1904,368;Inherit;False;Property;_UseAngleBasedFade;Use Angle Based Fade;25;0;Create;True;0;0;0;False;1;PerRendererData;False;0;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;213;-1904,592;Inherit;False;Property;_UseCameraDistanceFade;Use Camera Distance Fade;16;0;Create;True;0;0;0;False;1;PerRendererData;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-1552,512;Inherit;False;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;119;-1408,512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;-1856,-192;Inherit;False;DynamicRayVector;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-1264,512;Inherit;False;FinalAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-512,256;Inherit;False;123;DynamicRayVector;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;-464,176;Inherit;False;144;Style;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-272,96;Inherit;False;112;FinalAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;145;-257,176;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-366,-51;Inherit;False;99;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-33,-86;Float;False;True;-1;2;;0;0;Unlit;Distant Lands/Lumen/Light Ray;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;True;153;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;3;0;207;0
WireConnection;3;1;50;3
WireConnection;3;2;2;1
WireConnection;154;0;153;4
WireConnection;144;0;143;0
WireConnection;128;0;267;21
WireConnection;18;0;3;0
WireConnection;18;1;63;0
WireConnection;102;0;18;0
WireConnection;161;0;204;0
WireConnection;161;2;163;0
WireConnection;161;3;209;6
WireConnection;103;1;217;0
WireConnection;108;1;161;0
WireConnection;147;0;148;0
WireConnection;147;2;125;0
WireConnection;95;1;234;0
WireConnection;213;1;215;0
WireConnection;111;0;95;0
WireConnection;111;1;103;0
WireConnection;111;2;213;0
WireConnection;111;3;108;0
WireConnection;111;4;147;0
WireConnection;111;5;110;0
WireConnection;119;0;111;0
WireConnection;123;0;267;0
WireConnection;112;0;119;0
WireConnection;145;0;146;0
WireConnection;145;2;124;0
WireConnection;0;2;100;0
WireConnection;0;9;113;0
WireConnection;0;11;145;0
ASEEND*/
//CHKSM=172AEB2385F30F1C6A1B0F22CE9D2B634618B336