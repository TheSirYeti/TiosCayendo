// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RingShader"
{
	Properties
	{
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color1 = IsGammaSpace() ? float4(1,0.7253394,0,0) : float4(1,0.4849251,0,0);
			float4 color2 = IsGammaSpace() ? float4(0.9137255,0.9087271,0.1411765,0) : float4(0.8148467,0.8047925,0.01764196,0);
			float mulTime60 = _Time.y * 0.35;
			float4 lerpResult72 = lerp( color1 , color2 , step( sin( ( ( ( 1.0 - ( ( distance( i.uv_texcoord , float2( 0.5,0.5 ) ) * 4.84 ) * sqrt( ( ( 6.28318548202515 / 1.0 ) * 1.0 ) ) ) ) + mulTime60 ) * 5.04 ) ) , 0.0 ));
			o.Albedo = ( saturate( lerpResult72 ) * 0.75 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
532;73;938;655;-141.9698;344.4636;1;False;False
Node;AmplifyShaderEditor.CommentaryNode;51;-1751.856,150.2309;Inherit;False;714.442;304.2192;Wavelenght (w) =  sqrt((2*pi/L)*G);6;57;56;55;54;53;52;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1717.212,282.7748;Inherit;False;Constant;_Lenght;Lenght;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;53;-1679.973,205.256;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1553.159,305.4908;Inherit;False;Constant;_G;G;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;55;-1537.972,206.256;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1667.752,-470.9903;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;10;-1589.042,-161.1378;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1349.531,198.5934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-1295.711,-97.11801;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;4.84;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;12;-1358.772,-335.5665;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-1109.904,-287.6711;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;57;-1172.388,202.1089;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-688.9474,-16.42697;Inherit;False;Constant;_Speed;Speed;5;0;Create;True;0;0;False;0;False;0.35;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-774.7551,-286.0039;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;60;-439.8106,-17.65811;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;59;-476.442,-287.5792;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;-179.92,-239.8164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-98.99878,36.27363;Inherit;False;Constant;_RingAmount;RingAmount;6;0;Create;True;0;0;False;0;False;5.04;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;157.9927,-186.3147;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;71;381.5098,-195.7439;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;463.8724,343.0246;Inherit;False;Constant;_SecondaryColor;SecondaryColor;0;0;Create;True;0;0;False;0;False;0.9137255,0.9087271,0.1411765,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;450.3616,107.1766;Inherit;False;Constant;_BackgroundColor;BackgroundColor;0;0;Create;True;0;0;False;0;False;1,0.7253394,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;79;576.0953,-196.3689;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;72;790.7061,-57.98469;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;73;1062.38,-56.21635;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;1031.173,252.2716;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;False;0;False;0.75;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;1287.26,-33.90364;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1509.824,-69.70013;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;RingShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;53;0
WireConnection;55;1;52;0
WireConnection;56;0;55;0
WireConnection;56;1;54;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;75;0;12;0
WireConnection;75;1;74;0
WireConnection;57;0;56;0
WireConnection;58;0;75;0
WireConnection;58;1;57;0
WireConnection;60;0;64;0
WireConnection;59;0;58;0
WireConnection;61;0;59;0
WireConnection;61;1;60;0
WireConnection;62;0;61;0
WireConnection;62;1;65;0
WireConnection;71;0;62;0
WireConnection;79;0;71;0
WireConnection;72;0;1;0
WireConnection;72;1;2;0
WireConnection;72;2;79;0
WireConnection;73;0;72;0
WireConnection;78;0;73;0
WireConnection;78;1;77;0
WireConnection;0;0;78;0
ASEEND*/
//CHKSM=61D48C97BA0E470011A985EA97AF31A1C8BD37E3