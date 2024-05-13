// 创建者:   Harling
// 创建时间: 2023-05-19 10:57:42
// 备注:     由PIToolKit工具生成

Shader "PIToolKit/UI/TextMesh/TextShader" 
{
	Properties 
	{
		_MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
		[Toggle(_BillBoard)]_Board("BillBoard",int)=0
	}

	CGINCLUDE
	#pragma target 4.0
	#pragma multi_compile _ _BillBoard
	#pragma multi_compile_instancing
	#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
    #include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
	UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
	UNITY_INSTANCING_BUFFER_END(Props)

	struct appdata_t 
	{
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

	struct v2f
	{
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
		UNITY_VERTEX_INPUT_INSTANCE_ID 
	};
	
	void Vert(appdata_t adb,uint id:SV_INSTANCEID,out v2f o,out float4 pos:POSITION)
	{
		UNITY_SETUP_INSTANCE_ID(adb);
		UNITY_TRANSFER_INSTANCE_ID(adb, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		///////广告板
		#if _BillBoard
			float3 normal = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1)).xyz;
			normal = normalize(normal);
			float3 up = abs(normal.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
			float3 right = normalize(cross(normal, up));
			up = normalize(cross(right, normal));

			float3x3 m = { right, up, normal };
			m=transpose(m);
			adb.vertex.xyz=mul(m,adb.vertex.xyz);
		#endif
		/////////

        pos = UnityObjectToClipPos(adb.vertex);
        o.color = adb.color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
        o.texcoord = TRANSFORM_TEX(adb.texcoord,_MainTex);
	}

	void Frag(v2f data,out fixed4 col:SV_TARGET)
	{
		UNITY_SETUP_INSTANCE_ID(data);

		col = data.color;
        col.a *= tex2D(_MainTex, data.texcoord).a;
//      col.a = (col.a + tex2D(_MainTex, data.texcoord).a)/2;
	}

	ENDCG

	SubShader 
	{
		Tags 
		{
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
		pass
		{
			Lighting Off 
			Cull Off 
			ZTest LEqual 
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			ENDCG
		}

	}
	FallBack "Diffuse"
}
