// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/CustomOutlineToon"
{
	Properties
	{
		[NoScaleOffset]_BaseColorRGBOutlineWidthA("Base Color (RGB) Outline Width (A)", 2D) = "gray" {}
		_BaseTint("Base Tint", Color) = (1,1,1,0)
		_BaseCellSharpness("Base Cell Sharpness", Range( 0.01 , 1)) = 0.01
		_BaseCellOffset("Base Cell Offset", Range( -1 , 1)) = 0
		_IndirectDiffuseContribution("Indirect Diffuse Contribution", Range( 0 , 1)) = 1
		_ShadowContribution("Shadow Contribution", Range( 0 , 1)) = 0.5
		[NoScaleOffset]_Highlight("Highlight", 2D) = "white" {}
		[HDR]_HighlightTint("Highlight Tint", Color) = (1,1,1,1)
		_HighlightCellOffset("Highlight Cell Offset", Range( -1 , -0.5)) = -0.95
		_HighlightCellSharpness("Highlight Cell Sharpness", Range( 0.001 , 1)) = 0.01
		_IndirectSpecularContribution("Indirect Specular Contribution", Range( 0 , 1)) = 1
		[Toggle(_STATICHIGHLIGHTS_ON)] _StaticHighLights("Static HighLights", Float) = 0
		[Normal][NoScaleOffset]_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Range( 0 , 1)) = 1
		[Toggle(_OUTLINELINE_TANGENT_ON)] _OutlineTangent("outline_custom_tangent", Float) = 0
		_OutlineTint("Outline Tint", Color) = (0.5294118,0.5294118,0.5294118,0)
		_OutlineWidth("Outline Width", Range( 0.2 , 0)) = 0.02
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		#pragma shader_feature _OUTLINELINE_TANGENT_ON
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "/cginc/Outline.cginc"
		
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};
		uniform float _OutlineWidth;
		uniform sampler2D _BaseColorRGBOutlineWidthA;
		uniform float _IndirectDiffuseContribution;
		uniform float _NormalScale;
		uniform sampler2D _NormalMap;
		uniform float _BaseCellOffset;
		uniform float _BaseCellSharpness;
		uniform float _ShadowContribution;
		uniform float4 _BaseTint;
		uniform float4 _OutlineTint;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			#if _OUTLINELINE_TANGENT_ON
				BackFacingTangent(v, _BaseColorRGBOutlineWidthA, _OutlineWidth, 1);
			#else
				BackFacing(v, _BaseColorRGBOutlineWidthA, _OutlineWidth, 1);
			#endif
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float3 temp_cast_0 = (1.0).xxx;
			float3 lerpResult117 = lerp( temp_cast_0 , float3(0,0,0) , _IndirectDiffuseContribution);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_214_0 = ( 1.0 - ( ( 1.0 - 1 ) * _WorldSpaceLightPos0.w ) );
			float2 uv_NormalMap82 = i.uv_texcoord;
			float3 normalizeResult170 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap82 ), _NormalScale ) )) );
			float3 NewNormals220 = normalizeResult170;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult54 = dot( NewNormals220 , ase_worldlightDir );
			float NdotL236 = dotResult54;
			float lerpResult159 = lerp( temp_output_214_0 , ( saturate( ( ( NdotL236 + _BaseCellOffset ) / _BaseCellSharpness ) ) * 1 ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA76 = i.uv_texcoord;
			float4 tex2DNode76 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA76 );
			float3 BaseColor253 = ( ( ( lerpResult117 * ase_lightColor.a * temp_output_214_0 ) + ( ase_lightColor.rgb * lerpResult159 ) ) * (( tex2DNode76 * _BaseTint )).rgb );
			o.Emission = ( BaseColor253 * (_OutlineTint).rgb );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _STATICHIGHLIGHTS_ON
		#pragma surface surf StandardCustomLighting keepalpha vertex:vertexDataFunc 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _IndirectDiffuseContribution;
		uniform float _NormalScale;
		uniform sampler2D _NormalMap;
		uniform float _BaseCellOffset;
		uniform float _BaseCellSharpness;
		uniform float _ShadowContribution;
		uniform sampler2D _BaseColorRGBOutlineWidthA;
		uniform float4 _BaseTint;
		uniform float4 _HighlightTint;
		uniform sampler2D _Highlight;
		uniform float _IndirectSpecularContribution;
		uniform float _HighlightCellOffset;
		uniform float _HighlightCellSharpness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 temp_cast_1 = (1.0).xxx;
			float2 uv_NormalMap82 = i.uv_texcoord;
			float3 normalizeResult170 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap82 ), _NormalScale ) )) );
			float3 NewNormals220 = normalizeResult170;
			float3 indirectNormal106 = NewNormals220;
			float2 uv_Highlight81 = i.uv_texcoord;
			float4 temp_output_184_0 = ( _HighlightTint * tex2D( _Highlight, uv_Highlight81 ) );
			float temp_output_189_0 = (temp_output_184_0).a;
			Unity_GlossyEnvironmentData g106 = UnityGlossyEnvironmentSetup( temp_output_189_0, data.worldViewDir, indirectNormal106, float3(0,0,0));
			float3 indirectSpecular106 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal106, g106 );
			float3 lerpResult187 = lerp( temp_cast_1 , indirectSpecular106 , _IndirectSpecularContribution);
			float3 HighlightColor249 = (temp_output_184_0).rgb;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 LightColorFalloff227 = ( ase_lightColor.rgb * ase_lightAtten );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult4_g3 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float dotResult62 = dot( normalizeResult4_g3 , NewNormals220 );
			float dotResult54 = dot( NewNormals220 , ase_worldlightDir );
			float NdotL236 = dotResult54;
			#ifdef _STATICHIGHLIGHTS_ON
				float staticSwitch195 = NdotL236;
			#else
				float staticSwitch195 = dotResult62;
			#endif
			float3 temp_cast_2 = (1.0).xxx;
			UnityGI gi115 = gi;
			float3 diffNorm115 = NewNormals220;
			gi115 = UnityGI_Base( data, 1, diffNorm115 );
			float3 indirectDiffuse115 = gi115.indirect.diffuse + diffNorm115 * 0.0001;
			float3 lerpResult117 = lerp( temp_cast_2 , indirectDiffuse115 , _IndirectDiffuseContribution);
			float temp_output_214_0 = ( 1.0 - ( ( 1.0 - ase_lightAtten ) * _WorldSpaceLightPos0.w ) );
			float lerpResult159 = lerp( temp_output_214_0 , ( saturate( ( ( NdotL236 + _BaseCellOffset ) / _BaseCellSharpness ) ) * ase_lightAtten ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA76 = i.uv_texcoord;
			float4 tex2DNode76 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA76 );
			float3 BaseColor253 = ( ( ( lerpResult117 * ase_lightColor.a * temp_output_214_0 ) + ( ase_lightColor.rgb * lerpResult159 ) ) * (( tex2DNode76 * _BaseTint )).rgb );
			c.rgb = ( ( lerpResult187 * HighlightColor249 * LightColorFalloff227 * pow( temp_output_189_0 , 1.5 ) * saturate( ( ( staticSwitch195 + _HighlightCellOffset ) / ( ( 1.0 - temp_output_189_0 ) * _HighlightCellSharpness ) ) ) ) + BaseColor253 );
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float3 temp_cast_0 = (1.0).xxx;
			float3 lerpResult117 = lerp( temp_cast_0 , float3(0,0,0) , _IndirectDiffuseContribution);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_214_0 = ( 1.0 - ( ( 1.0 - 1 ) * _WorldSpaceLightPos0.w ) );
			float2 uv_NormalMap82 = i.uv_texcoord;
			float3 normalizeResult170 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap82 ), _NormalScale ) )) );
			float3 NewNormals220 = normalizeResult170;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult54 = dot( NewNormals220 , ase_worldlightDir );
			float NdotL236 = dotResult54;
			float lerpResult159 = lerp( temp_output_214_0 , ( saturate( ( ( NdotL236 + _BaseCellOffset ) / _BaseCellSharpness ) ) * 1 ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA76 = i.uv_texcoord;
			float4 tex2DNode76 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA76 );
			float3 BaseColor253 = ( ( ( lerpResult117 * ase_lightColor.a * temp_output_214_0 ) + ( ase_lightColor.rgb * lerpResult159 ) ) * (( tex2DNode76 * _BaseTint )).rgb );
			o.Albedo = BaseColor253;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
1927;127;1906;806;-1267.506;2328.159;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;226;-803.833,-214.5792;Float;False;1370.182;280;Comment;5;82;52;170;220;109;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-753.8331,-138.0697;Float;False;Property;_NormalScale;Normal Scale;13;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-422.226,-164.5792;Float;True;Property;_NormalMap;Normal Map;12;2;[Normal];[NoScaleOffset];Create;True;0;0;False;0;None;3b3a027812aab9749a9ca6ec7ebc3500;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;52;-103.4431,-159.3391;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;170;138.1924,-160.2827;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;241;-676.3679,366.5508;Float;False;835.6508;341.2334;Comment;4;53;223;54;236;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;220;323.3487,-158.8317;Float;False;NewNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;223;-599.3095,416.5508;Float;False;220;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;53;-626.368,528.7842;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;54;-302.0764,453.3497;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;242;1324.039,459.9366;Float;False;2744.931;803.0454;Comment;25;253;158;130;235;73;182;76;133;107;159;162;160;214;74;213;215;207;57;60;58;127;59;237;256;257;Base Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;236;-83.71747,456.6653;Float;False;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;1374.039,641.3355;Float;False;Property;_BaseCellOffset;Base Cell Offset;3;0;Create;True;0;0;False;0;0;-0.18;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;1374.247,533.2394;Float;False;236;NdotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;127;1631.055,790.9418;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;1676.114,631.0218;Float;False;Property;_BaseCellSharpness;Base Cell Sharpness;2;0;Create;True;0;0;False;0;0.01;0.562;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;1657.487,534.5102;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;215;1944.082,816.0775;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;57;1956.552,537.3538;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;207;1626.013,899.7625;Float;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;233;1355.396,-187.3015;Float;False;828.4254;361.0605;Comment;5;115;118;117;119;225;Indirect Diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;2134.626,851.7131;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;251;648.5698,-1229.416;Float;False;2234.221;738.9581;Comment;19;184;180;249;246;181;177;81;172;175;61;222;239;62;195;174;173;171;261;260;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;74;2114.005,542.3831;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;225;1405.396,-42.92128;Float;False;220;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;214;2324.502,854.6982;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;81;1260.552,-1012.602;Float;True;Property;_Highlight;Highlight;6;1;[NoScaleOffset];Create;True;0;0;False;0;None;b10566cb657e0f34985df044d2e19c09;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;177;1329.485,-1179.416;Float;False;Property;_HighlightTint;Highlight Tint;7;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;118;1659.391,58.75889;Float;False;Property;_IndirectDiffuseContribution;Indirect Diffuse Contribution;4;0;Create;True;0;0;False;0;1;0.456;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;1785.008,-137.3016;Float;False;Constant;_Float4;Float 4;20;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;2351.156,541.2684;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;115;1688.455,-41.32622;Float;False;World;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;162;2155.415,983.4974;Float;False;Property;_ShadowContribution;Shadow Contribution;5;0;Create;True;0;0;False;0;0.5;0.457;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;2975.755,1079.272;Float;False;Property;_BaseTint;Base Tint;1;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;222;762.1332,-605.4583;Float;False;220;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;117;1999.821,-60.9863;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;1659.207,-1057.273;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;107;2626.905,541.7966;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;76;2937.31,875.5428;Float;True;Property;_BaseColorRGBOutlineWidthA;Base Color (RGB) Outline Width (A);0;1;[NoScaleOffset];Create;True;0;0;False;0;None;b10566cb657e0f34985df044d2e19c09;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;159;2707.211,856.7739;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;61;698.5698,-733.562;Float;False;Blinn-Phong Half Vector;-1;;3;91a149ac9d615be429126c95e20753ce;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;248;1892.391,-1558.685;Float;False;287;165;Comment;1;189;Spec/Smooth;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;62;1043.79,-679.1187;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;182;3048.637,509.9366;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;240;-635.8909,943.4141;Float;False;717.6841;295.7439;Comment;4;229;228;230;227;Light Falloff;0.9947262,1,0.6176471,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;189;1942.391,-1508.685;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;256;3276.777,970.2915;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;3050.939,734.3177;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;239;1003.499,-845.3307;Float;False;236;NdotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;228;-540.1871,993.4141;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightAttenuation;229;-585.8909,1129.158;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;174;1799.042,-648.0834;Float;False;Property;_HighlightCellSharpness;Highlight Cell Sharpness;9;0;Create;True;0;0;False;0;0.01;0.01;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;3307.784,623.0619;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;195;1271.122,-784.4333;Float;False;Property;_StaticHighLights;Static HighLights;11;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;247;1687.587,-2100.743;Float;False;1008.755;365.3326;Comment;5;224;106;188;186;187;Indirect Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;173;1405.781,-660.3005;Float;False;Property;_HighlightCellOffset;Highlight Cell Offset;8;0;Create;True;0;0;False;0;-0.95;-0.54;-1;-0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;235;3423.945,954.0895;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;261;2021.558,-885.707;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;219;3993.704,1541.79;Float;False;1039.617;429.9737;Comment;8;259;200;258;83;185;245;254;192;Custom Outline;1,0.6029412,0.7097364,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;2116.472,-654.7482;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;1737.587,-1980.103;Float;False;220;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;230;-351.8918,1059.575;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;1760.558,-788.2134;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;3608.84,749.2598;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-177.207,1056.042;Float;False;LightColorFalloff;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;257;3389.934,1086.04;Float;False;OutlineCustomWidth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;186;2188.282,-2050.743;Float;False;Constant;_Float5;Float 5;20;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;172;2108.795,-795.9062;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;192;4054.217,1683.832;Float;False;Property;_OutlineTint;Outline Tint;14;0;Create;True;0;0;False;0;0.5294118,0.5294118,0.5294118,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;188;2094.48,-1850.412;Float;False;Property;_IndirectSpecularContribution;Indirect Specular Contribution;10;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectSpecularLight;106;2161.653,-1963.519;Float;False;World;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;253;3801.573,744.8395;Float;False;BaseColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;180;1898.907,-1053.456;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;263;2277.378,-1363.768;Float;False;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;187;2512.345,-1997.898;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;200;4326.404,1785.057;Float;False;Property;_OutlineWidth;Outline Width;15;0;Create;True;0;0;False;0;0.02;2;0.2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;175;2324.68,-792.1935;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;246;2204.838,-950.8322;Float;False;227;LightColorFalloff;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;2211.476,-1055.947;Float;False;HighlightColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;4345.463,1881.319;Float;False;257;OutlineCustomWidth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;254;4299.977,1587.032;Float;False;253;BaseColor;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;245;4353.897,1678.073;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;258;4668.784,1790.126;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;4617.75,1638.073;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.3382353,0.3382353,0.3382353;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;2713.791,-1059.016;Float;False;5;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;4527.494,743.6974;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;252;4984.134,546.408;Float;False;253;BaseColor;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OutlineNode;83;4783.315,1639.851;Float;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5309.991,551.4377;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;ASESampleShaders/CustomOutlineToon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.03;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;82;5;109;0
WireConnection;52;0;82;0
WireConnection;170;0;52;0
WireConnection;220;0;170;0
WireConnection;54;0;223;0
WireConnection;54;1;53;0
WireConnection;236;0;54;0
WireConnection;58;0;237;0
WireConnection;58;1;59;0
WireConnection;215;0;127;0
WireConnection;57;0;58;0
WireConnection;57;1;60;0
WireConnection;213;0;215;0
WireConnection;213;1;207;2
WireConnection;74;0;57;0
WireConnection;214;0;213;0
WireConnection;160;0;74;0
WireConnection;160;1;127;0
WireConnection;115;0;225;0
WireConnection;117;0;119;0
WireConnection;117;1;115;0
WireConnection;117;2;118;0
WireConnection;184;0;177;0
WireConnection;184;1;81;0
WireConnection;159;0;214;0
WireConnection;159;1;160;0
WireConnection;159;2;162;0
WireConnection;62;0;61;0
WireConnection;62;1;222;0
WireConnection;182;0;117;0
WireConnection;182;1;107;2
WireConnection;182;2;214;0
WireConnection;189;0;184;0
WireConnection;256;0;76;0
WireConnection;256;1;73;0
WireConnection;133;0;107;1
WireConnection;133;1;159;0
WireConnection;130;0;182;0
WireConnection;130;1;133;0
WireConnection;195;1;62;0
WireConnection;195;0;239;0
WireConnection;235;0;256;0
WireConnection;261;0;189;0
WireConnection;260;0;261;0
WireConnection;260;1;174;0
WireConnection;230;0;228;1
WireConnection;230;1;229;0
WireConnection;171;0;195;0
WireConnection;171;1;173;0
WireConnection;158;0;130;0
WireConnection;158;1;235;0
WireConnection;227;0;230;0
WireConnection;257;0;76;4
WireConnection;172;0;171;0
WireConnection;172;1;260;0
WireConnection;106;0;224;0
WireConnection;106;1;189;0
WireConnection;253;0;158;0
WireConnection;180;0;184;0
WireConnection;263;0;189;0
WireConnection;187;0;186;0
WireConnection;187;1;106;0
WireConnection;187;2;188;0
WireConnection;175;0;172;0
WireConnection;249;0;180;0
WireConnection;245;0;192;0
WireConnection;258;0;200;0
WireConnection;258;1;259;0
WireConnection;185;0;254;0
WireConnection;185;1;245;0
WireConnection;181;0;187;0
WireConnection;181;1;249;0
WireConnection;181;2;246;0
WireConnection;181;3;263;0
WireConnection;181;4;175;0
WireConnection;163;0;181;0
WireConnection;163;1;253;0
WireConnection;83;0;185;0
WireConnection;83;1;258;0
WireConnection;0;0;252;0
WireConnection;0;13;163;0
WireConnection;0;11;83;0
ASEEND*/
//CHKSM=3109C9F778B926D1D9ED16A97EB373AB1F560194