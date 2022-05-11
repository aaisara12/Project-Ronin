// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Lumen/Flare"
{
	Properties
	{
		[HDR][PerRendererData]_MainColor("Main Color", Color) = (1,1,1,0.454902)
		[PerRendererData]_CameraDistanceFadeStart("Camera Distance Fade Start", Float) = 20
		[PerRendererData]_CameraDistanceFadeEnd("Camera Distance Fade End", Float) = 30
		[HideInInspector]_Intensity("Intensity", Range( 0 , 1)) = 1
		[PerRendererData]_RenderOffset("Render Offset", Float) = 0
		[PerRendererData]_CameraDepthFadeStart("Camera Depth Fade Start", Float) = 0
		[PerRendererData]_CameraDepthFadeEnd("Camera Depth Fade End", Float) = 2
		[PerRendererData][Toggle]_UseSceneDepthFade("UseSceneDepthFade", Float) = 1
		[PerRendererData][Toggle]_UseZScaleFade("UseZScaleFade", Float) = 1
		[PerRendererData][Toggle]_UseCameraDistanceFade("UseCameraDistanceFade", Float) = 1
		[PerRendererData]_DepthFadeStartDistance("Depth Fade Start Distance", Float) = 0
		[PerRendererData]_DepthFadeEndDistance("Depth Fade End Distance", Float) = 2
		[PerRendererData][Toggle]_UseCameraDepthFade("UseCameraDepthFade", Float) = 1
		[PerRendererData][Toggle]_UseVariation("UseVariation", Float) = 1
		[PerRendererData]_VariationSpeed("Variation Speed", Range( 0 , 10)) = 1
		[PerRendererData]_VariationScale("Variation Scale", Float) = 1
		[HDR][PerRendererData]_VariationColor("Variation Color", Color) = (1,1,1,0.454902)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "DisableBatching" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float4 vertexColor : COLOR;
		};

		uniform float _RenderOffset;
		uniform float _UseVariation;
		uniform float4 _MainColor;
		uniform float4 _VariationColor;
		uniform float _VariationSpeed;
		uniform float _VariationScale;
		uniform float _UseCameraDepthFade;
		uniform float _CameraDepthFadeStart;
		uniform float _CameraDepthFadeEnd;
		uniform float _UseCameraDistanceFade;
		uniform float _CameraDistanceFadeEnd;
		uniform float _CameraDistanceFadeStart;
		uniform float _UseZScaleFade;
		uniform float _UseSceneDepthFade;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeStartDistance;
		uniform float _DepthFadeEndDistance;
		uniform float _Intensity;


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
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = normalize ( UNITY_MATRIX_V._m10_m11_m12 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.normal = normalize( mul( float4( v.normal , 0 ), rotationCamMatrix )).xyz;
			v.tangent.xyz = normalize( mul( float4( v.tangent.xyz , 0 ), rotationCamMatrix )).xyz;
			v.vertex.x *= length( unity_ObjectToWorld._m00_m10_m20 );
			v.vertex.y *= length( unity_ObjectToWorld._m01_m11_m21 );
			v.vertex.z *= length( unity_ObjectToWorld._m02_m12_m22 );
			v.vertex = mul( v.vertex, rotationCamMatrix );
			v.vertex.xyz += unity_ObjectToWorld._m03_m13_m23;
			//Need to nullify rotation inserted by generated surface shader;
			v.vertex = mul( unity_WorldToObject, v.vertex );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 transform26_g1 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float2 appendResult28_g1 = (float2(( transform26_g1.x + transform26_g1.y ) , transform26_g1.z));
			float2 UVPos31_g1 = appendResult28_g1;
			float mulTime6_g1 = _Time.y * _VariationSpeed;
			float simplePerlin2D18_g1 = snoise( (UVPos31_g1*1.0 + ( mulTime6_g1 * -0.5 ))*( 2.0 / _VariationScale ) );
			simplePerlin2D18_g1 = simplePerlin2D18_g1*0.5 + 0.5;
			float simplePerlin2D19_g1 = snoise( (UVPos31_g1*1.0 + mulTime6_g1)*( 1.0 / _VariationScale ) );
			simplePerlin2D19_g1 = simplePerlin2D19_g1*0.5 + 0.5;
			float4 lerpResult16 = lerp( _MainColor , ( _MainColor * _VariationColor ) , saturate( min( simplePerlin2D18_g1 , simplePerlin2D19_g1 ) ));
			float4 RayColor22 = ( _UseVariation == 0.0 ? _MainColor : lerpResult16 );
			o.Emission = RayColor22.rgb;
			float4 transform9_g98 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float4 transform9_g97 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 ase_parentObjectScale = (1.0/float3( length( unity_WorldToObject[ 0 ].xyz ), length( unity_WorldToObject[ 1 ].xyz ), length( unity_WorldToObject[ 2 ].xyz ) ));
			float isOrtho31 = unity_OrthoParams.w;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth4_g92 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth4_g92 = ( screenDepth4_g92 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 );
			float screenDepth17_g93 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth17_g93 = abs( ( screenDepth17_g93 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float depthToLinear18_g93 = Linear01Depth(distanceDepth17_g93);
			float luminance26 = Luminance(RayColor22.rgb);
			float RayPreAlpha43 = ( ( luminance26 * RayColor22.a * i.vertexColor.r ) * _Intensity );
			float FinalAlpha57 = saturate( ( ( _UseCameraDepthFade == 1.0 ? saturate( (0.0 + (distance( transform9_g98 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDepthFadeStart) * (1.0 - 0.0) / (_CameraDepthFadeEnd - _CameraDepthFadeStart)) ) : 1.0 ) * ( _UseCameraDistanceFade == 1.0 ? saturate( (0.0 + (distance( transform9_g97 , float4( _WorldSpaceCameraPos , 0.0 ) ) - _CameraDistanceFadeEnd) * (1.0 - 0.0) / (_CameraDistanceFadeStart - _CameraDistanceFadeEnd)) ) : 1.0 ) * saturate( ( _UseZScaleFade == 1.0 ? ase_parentObjectScale.z : 1.0 ) ) * ( _UseSceneDepthFade == 1.0 ? ( isOrtho31 == 0.0 ? saturate( (0.0 + (distanceDepth4_g92 - _DepthFadeStartDistance) * (1.0 - 0.0) / (_DepthFadeEndDistance - _DepthFadeStartDistance)) ) : saturate( ( 1.0 - depthToLinear18_g93 ) ) ) : 1.0 ) * RayPreAlpha43 ) );
			o.Alpha = FinalAlpha57;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=18934
0;1085.714;2194;600;3462.415;482.6822;1.173322;True;False
Node;AmplifyShaderEditor.CommentaryNode;10;-2704,-368;Inherit;False;1115.711;544.7944;;7;100;22;88;89;16;15;12;Ray Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;15;-2640,-48;Inherit;False;Property;_VariationColor;Variation Color;22;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;1.763311,1.032861,0.6404479,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-2640,-224;Inherit;False;Property;_MainColor;Main Color;0;2;[HDR];[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,0.454902;2.354159,2.354159,2.354159,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;103;-2400,16;Inherit;False;Variation;18;;1;487039bf64ba820499085da4bb1fd9c4;1,34,0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-2384,-96;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;16;-2208,-144;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-2207,-288;Inherit;False;Property;_UseVariation;UseVariation;17;2;[PerRendererData];[Toggle];Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;88;-2016,-256;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-1840,-256;Inherit;False;RayColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2704,352;Inherit;False;1103.87;535.4168;;8;43;41;36;33;30;29;26;24;Pre-Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2672,400;Inherit;False;22;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;25;-1520,-368;Inherit;False;554.9871;313.8767;;3;31;27;84;Variables;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;30;-2480,656;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;29;-2432,512;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OrthoParams;27;-1456,-208;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LuminanceNode;26;-2432,416;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;32;-1568,352;Inherit;False;1432.883;1062.025;;19;55;57;54;97;96;94;95;79;81;93;92;47;51;39;42;87;91;90;102;Fading Functions;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2304,768;Inherit;False;Property;_Intensity;Intensity;5;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;0.263;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-1232,-208;Inherit;False;isOrtho;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-2192,512;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2016,608;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;87;-1473,1088;Inherit;False;Scene Depth Fade;13;;92;b80e7cce07f3ff241b5ed9e75228c4d5;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;81;-1264,912;Inherit;False;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;39;-1472,1168;Inherit;False;Orthographic Fade;-1;;93;07ff034ed2c5b5b4783cf486c6ebd7b0;0;0;1;FLOAT;6
Node;AmplifyShaderEditor.RangedFloatNode;95;-1264,832;Inherit;False;Property;_UseZScaleFade;UseZScaleFade;11;2;[PerRendererData];[Toggle];Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-1472,992;Inherit;False;31;isOrtho;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;94;-1056,880;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;101;-1248,528;Inherit;False;Camera Depth Fade;7;;98;b003bbeb8eacda14fa920f9527150e3e;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1264,656;Inherit;False;Property;_UseCameraDistanceFade;UseCameraDistanceFade;12;2;[PerRendererData];[Toggle];Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-1872,608;Inherit;False;RayPreAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;102;-1264,752;Inherit;False;Camera Distance Fade;1;;97;f992419659205874689e17721bd92798;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-1232,1232;Inherit;False;Property;_UseSceneDepthFade;UseSceneDepthFade;10;2;[PerRendererData];[Toggle];Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-1248,448;Inherit;False;Property;_UseCameraDepthFade;UseCameraDepthFade;16;2;[PerRendererData];[Toggle];Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;47;-1232,1072;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;90;-976,464;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;97;-960,672;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;92;-960,1072;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-960,1264;Inherit;False;43;RayPreAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;79;-896,880;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-672,816;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;55;-512,816;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-368,816;Inherit;False;FinalAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-240,48;Inherit;False;22;RayColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;62;-240,176;Inherit;False;57;FinalAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1456,-320;Inherit;False;Property;_RenderOffset;Render Offset;6;1;[PerRendererData];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;;0;0;Unlit;Distant Lands/Lumen/Flare;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;False;False;False;False;False;Back;0;False;-1;7;False;6;False;1;False;-1;0;True;84;False;0;Custom;0.5;True;False;1;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;True;Spherical;True;True;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;100;0;12;0
WireConnection;100;1;15;0
WireConnection;16;0;12;0
WireConnection;16;1;100;0
WireConnection;16;2;103;0
WireConnection;88;0;89;0
WireConnection;88;2;12;0
WireConnection;88;3;16;0
WireConnection;22;0;88;0
WireConnection;29;0;24;0
WireConnection;26;0;24;0
WireConnection;31;0;27;4
WireConnection;36;0;26;0
WireConnection;36;1;29;3
WireConnection;36;2;30;1
WireConnection;41;0;36;0
WireConnection;41;1;33;0
WireConnection;94;0;95;0
WireConnection;94;2;81;3
WireConnection;43;0;41;0
WireConnection;47;0;42;0
WireConnection;47;2;87;0
WireConnection;47;3;39;6
WireConnection;90;0;91;0
WireConnection;90;2;101;0
WireConnection;97;0;96;0
WireConnection;97;2;102;0
WireConnection;92;0;93;0
WireConnection;92;2;47;0
WireConnection;79;0;94;0
WireConnection;54;0;90;0
WireConnection;54;1;97;0
WireConnection;54;2;79;0
WireConnection;54;3;92;0
WireConnection;54;4;51;0
WireConnection;55;0;54;0
WireConnection;57;0;55;0
WireConnection;0;2;61;0
WireConnection;0;9;62;0
ASEEND*/
//CHKSM=95D0F5B81DE11ECD2263E05BACA43CF2FAC06F51