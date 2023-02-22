// 创建者:   Harling
// 创建时间: 2020-10-15 20:11:06
// 备注:     全息Shader

Shader "Hidden/HoloGraphic" 
{ 
    Properties 
	{
		_MainTex("MainTex",2D)="white"{}
		[HDR]HoloCol("Color",COLOR)=(1,1,1,1)
		Ratio("Ratio",Range(0,1))=0.1
		LineWidth("LineWidth",Range(0,0.2))=0.05
		MaxRange("MaxRange",Range(0,1000))=100
	}

	CGINCLUDE

	#pragma target 4.0
	#pragma multi_compile_instancing
	
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed4 HoloCol;
	float Ratio;
	float LineWidth;
	float MaxRange;

	struct v2g
	{ 
		float4 Pos:SV_POSITION;
		float3 WP:NORMAL0;
		float2 UV:TEXCOORD0;
		float3 ViewVector:NORMAL1;
		float3 WorldNormal:NORMAL2;
		UNITY_VERTEX_OUTPUT_STEREO
		UNITY_VERTEX_INPUT_INSTANCE_ID 
    }; 
 
    struct g2f 
    { 
		float4 Pos:SV_POSITION;
		float4 Dis:NORMAL0;
		float2 UV:TEXCOORD0;
		float3 WP:TEXCOORD1;
		float3 ViewVector:NORMAL1;
		float3 WorldNormal:NORMAL2;
		UNITY_VERTEX_OUTPUT_STEREO
		UNITY_VERTEX_INPUT_INSTANCE_ID 
    }; 
	float InverseLerp(float min,float max,float value )
	{
		return (value-min)/(max-min);
	}
	//线框渲染
	void vert (appdata_base adb,uint id:SV_INSTANCEID,out v2g o) 
    { 
		UNITY_SETUP_INSTANCE_ID(adb);
		UNITY_TRANSFER_INSTANCE_ID(adb, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.Pos=UnityObjectToClipPos(adb.vertex);
		o.WP=mul(unity_ObjectToWorld,adb.vertex).xyz;
		o.UV=adb.texcoord.xy*_MainTex_ST.xy+_MainTex_ST.zw;

		o.WorldNormal=UnityObjectToWorldNormal(adb.normal);
		o.ViewVector=normalize(_WorldSpaceCameraPos.xyz -o.WP);

    } 
	//定义每次调用图元着色器所允许输出的最大顶点数目
    [maxvertexcount(3)] 
    void geom(triangle v2g tris[3],inout TriangleStream<g2f> outStream)
	{ 
		float3 v01 = tris[1].WP - tris[0].WP;
		float3 v02 = tris[2].WP - tris[0].WP;
		float3 v12 = tris[2].WP - tris[1].WP;

		float area =length(cross(v01,v02));
		float l12=length(v12);
		float l02=length(v02);
		float l01=length(v01);

		//计算每个点到三条边的距离
		float dist0 = area / l12;
		float dist1 = area / l02;
		float dist2 = area / l01;
		float maxlen=max(l12,max(l02,l01));

		g2f o[3];
		for(int i=0;i<3;i++)
		{
			o[i].UV = tris[i].UV;
			o[i].Pos = tris[i].Pos;
			o[i].WorldNormal=tris[i].WorldNormal;
			o[i].ViewVector=tris[i].ViewVector;
			o[i].WP=tris[i].WP;
			o[i].Dis = float4(dist0,dist1,dist2,0);

			float4 temp= float4(step(abs(i-0),0),step(abs(i-1),0),step(abs(i-2),0),0);
			o[i].Dis*=temp;

			UNITY_SETUP_INSTANCE_ID(tris[i]);
			UNITY_TRANSFER_INSTANCE_ID(tris[i], o[i]);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o[i]);

		}
		//将最长边的距离设置为1000000剔除
		if(maxlen==l12)o[0].Dis.x=1000000;
		else if(maxlen==l02)o[1].Dis.y=1000000;
		else if(maxlen==l01)o[2].Dis.z=1000000;
		outStream.Append(o[0]);
		outStream.Append(o[1]);
		outStream.Append(o[2]);
		//表示当前图元构建完毕，下一次调用开始绘制新图元
        outStream.RestartStrip();
    } 
             
    void frag(g2f data,out fixed4 col:SV_TARGET)
	{
		UNITY_SETUP_INSTANCE_ID(data);
		float depth=distance(data.WP,_WorldSpaceCameraPos.xyz);


		float dis = min(data.Dis.x, min(data.Dis.y, data.Dis.z));
		float width=1-saturate(InverseLerp(0,LineWidth*depth/2,dis));
		float fresnel= 1-saturate(dot(data.WorldNormal,data.ViewVector));
		fresnel=pow(fresnel,5);
		col=1;

		depth=saturate(1.0-depth/MaxRange);
		depth=pow(depth,8);
		col=tex2D(_MainTex,data.UV)*HoloCol;
		width=sign(width);
		col=lerp(col,HoloCol,width);
		col.a=lerp(col.a*Ratio*fresnel,HoloCol.a,width)*depth;
		clip(col.a-0.001);
	}
	ENDCG

	SubShader 
	{
		Tags {"RenderType" = "Opaque" "Queue"="Transparent"}
		pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off
			CGPROGRAM
			#pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
			ENDCG
		}
	}
	FallBack "Diffuse"
}
