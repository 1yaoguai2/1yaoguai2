#  API

## 1.API简介

Application Programming Interface 应用程序编程接口

[;]: ;

## 2.Web API

[学习]: https://developer.mozilla.org/zh-CN/docs/Learn/Getting_started_with_the_web/Publishing_your_website

## 3.新建Web API项目

| 步骤                                                         | 提示                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| 1.新建VS2019项目                                             | ASP.NET Core WEb API                                         |
| 2.测试项目默认的json                                         | 运行项目，try ,exd                                           |
| 3.更新 Properties\launchSettings.json 中，将 `launchUrl` 从 `"swagger"` 更新为 `"api/todoitems"` | 模型名称                                                     |
| 4.添加模型类，新建文件夹，再添加类                           | 模型类默认放在Models下，代码块1                              |
| 5.添加NuGet包。工具=>NuGet包管理器=>解决方案NuGet包          | 浏览下搜索Microsoft.EntityFrameworkCore.InMemory             |
| 6.添加TodoContext数据库上下文                                | Models下添加类，代码块2                                      |
| 7.注册数据库上下文                                           | 更新 Startup.cs的代码，代码块3                               |
| 8.构建控制器                                                 | 右键Controllers文件=>添加=>控制器=>API=>其操作使用Entity Framework的API控制器（添加其操作使用实体框架的API控制器） |
| 9.更新PostTodoItem中的 return语句                            | 代码块3                                                      |
| 10.开启测试                                                  | get,post,put,delete                                          |

```c#
//代码块1
namespace TodoApi.Models
{
    public class 类名1
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
```



```c#
//代码块2
using Microsoft.EntityFrameworkCore;

namespace 项目名：Models
{
    public class 类名2 : DbContext
    {
        public 类名2（DbContextOptions<类名2> options）: base(options)
        {
        }
        
        public DbSet<类名2> 类名2s {get; set;}
    }
}
```

```c#
//代码块4
// POST: api/TodoItems
[HttpPost]
public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
{
    _context.TodoItems.Add(todoItem);
    await _context.SaveChangesAsync();

    //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
}
```

```c#
//代码块3
// Unused usings removed
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
///+using Microsoft.EntityFrameworkCore;
///+using 项目名.Models;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ///+services.AddDbContext<TodoContext>(opt =>
                                        ///opt.UseInMemoryDatabase("TodoList")); //内存数据库（“名字”）
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoApi", Version = "v1" });
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

