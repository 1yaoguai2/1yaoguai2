### DOTS开发指南

1.*Authoring

```
//Authing文件
//类名来源于功能
//继承MonoBehavior，挂载到对象，将对象转化为
//功能所需要的外部参数
//Baker类
//GetEntity转换为DOTS对象
//添加组件并传递参数

using UnityEngine;
using Unity.Entities;

public class RotateAuthoring : MonoBehaviour
{
  public float value;

   protected class Baker : Baker<RotateAuthoring>
   {
       public override void Bake(RotateAuthoring authoring)
       {
           //该方法在实体生成时被调用，负责将MonoBehaviour上的属性数据转化为ECS组件
           Entity entity = GetEntity(TransformUsageFlags.Dynamic);
           
           //为实体添加 Rotate组件，并将作者的旋转速度值传递给它
           AddComponent(entity,new RotateComponent
           {
               value = authoring.value, //设置旋转速度
           });
       }
   }
}

```

2.*Component

```
//结构体，名称由功能定
//实现接口IComponentData
//数据（面向数据开发）

using Unity.Entities;

public struct RotateComponent : IComponentData
{
    public float value;
}
```

3.*System

```
//基于数据实现功能系统
//部分的结构体，名称由功能决定，特性[BurstCompile]
//实现接口 ISystem
//方法OnCreate（ref SysteamState state）
//方法OnUpdate(ref systeamState state),特性[BurstCompile]
//普通调用Foreach
//特殊调用，新建部分结构体*Job，继承IJobEntity，特性[BurstCompile]
//属性，实例化时赋值，也可以使用构造函数
//方法Execute(ref LoaclTransform LocalTransform,in RotateComponent rotateComponent)

//启动Job
//无依赖,单线程不并行，*Job.Schedule();
//有依赖，在多个线程上不并行，*Job.Schedule(state.Dependency);
//在多个线程上并行，*Job.ScheduleParallel();

using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct RotatingCubeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        //在系统创建时要求必须有RotateSpeed组件才能进行跟新
        //state.RequireForUpdate<Rotate>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //普通调用
         // foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateComponent> rotate)
        //          in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateComponent>>())
        // {
        //     localTransform.ValueRW =
        //         localTransform.ValueRO.RotateY(rotate.ValueRO.value * SystemAPI.Time.DeltaTime);
        //     Debug.Log($"旋转：localTransform:{localTransform.ValueRW.Rotation},rotate{rotate.ValueRO.value}");
        // }
        
        //创建旋转立方体的工作作业，并传递时间增量信息
        RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime  //获取每帧的时间增量
        };
        
        //调度工作作业
        //没有依赖关系，单线程执行，不支持并行
        // rotatingCubeJob.Schedule();
        //存在依赖关系，作业之间有序顺序执行需求，可能在多个线程上执行，取决于作业依赖， 不支持并行
        // state.Dependency = rotatingCubeJob.Schedule(state.Dependency);
        //在多个线程上并行
        rotatingCubeJob.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime; //存储时间增量，用于旋转计算

        //执行作业时具体操作
        public void Execute(ref LocalTransform localTransform, in RotateComponent rotateComponent)
        {
            float power = 1f;

            localTransform = localTransform.RotateY(rotateComponent.value * deltaTime * power);

            //Debug.Log($"旋转：localTransform:{localTransform.Rotation},rotate{rotateComponent.value}");
        }
    }

}
```

### 挂载和场景

将Authoring脚本挂载到对象，在需要的场景中右键New Sub Scene->Empty Scene,保存场景后，在场景中添加挂载脚本的对象。