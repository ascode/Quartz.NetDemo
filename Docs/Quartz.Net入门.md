# Net作业调度(一) -Quartz.Net入门
---
## 背景   
很多时候，项目需要在不同时刻，执行一个或很多个不同的作业。

Windows执行计划这时并不能很好的满足需求了，迫切需要一个更为强大，方便管理，集群部署的作业调度框架。  
## 介绍  
Quartz一个开源的作业调度框架，OpenSymphony的开源项目。Quartz.Net 是Quartz的C#移植版本。   

它一些很好的特性：  

1：支持集群，作业分组，作业远程管理。   

2：自定义精细的时间触发器，使用简单，作业和触发分离。  

3：数据库支持，可以寄宿Windows服务，WebSite，winform等。  
## 实战  
Quartz框架的一些基础概念解释：  

 　　Scheduler     作业调度器。  

 　　IJob             作业接口，继承并实现Execute， 编写执行的具体作业逻辑。  

　　JobBuilder       根据设置，生成一个详细作业信息(JobDetail)。  

　　TriggerBuilder   根据规则，生产对应的Trigger  

Nuget安装   
PM> Install-Package Quartz   
下面是简单使用例子，附带详细的注释：  
```
static void Main(string[] args)
       {
           //从工厂中获取一个调度器实例化
           IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
 
           scheduler.Start();       //开启调度器
 
           //==========例子1（简单使用）===========
 
           IJobDetail job1 = JobBuilder.Create<HelloJob>()  //创建一个作业
               .WithIdentity("作业名称", "作业组")
               .Build();
 
           ITrigger trigger1 = TriggerBuilder.Create()
                                       .WithIdentity("触发器名称", "触发器组")
                                       .StartNow()                        //现在开始
                                       .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                           .WithIntervalInSeconds(5)
                                           .RepeatForever())              //不间断重复执行
                                       .Build();
 
 
           scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
 
           //==========例子2 (执行时 作业数据传递，时间表达式使用)===========
 
           IJobDetail job2= JobBuilder.Create<DumbJob>()
                                       .WithIdentity("myJob", "group1")
                                       .UsingJobData("jobSays", "Hello World!")
                                       .Build();
 
 
           ITrigger trigger2 = TriggerBuilder.Create()
                                       .WithIdentity("mytrigger", "group1")
                                       .StartNow()
                                       .WithCronSchedule("/5 * * ? * *")    //时间表达式，5秒一次     
                                       .Build();
 
 
           scheduler.ScheduleJob(job2, trigger2);     
         
           //scheduler.Shutdown();         //关闭调度器。
       }
```
声明要执行的作业，HelloJob：  
```
/// <summary>
   /// 作业
   /// </summary>
   public class HelloJob : IJob
   {
       public void Execute(IJobExecutionContext context)
       {
           Console.WriteLine("作业执行!");
       }
   }
```
声明要执行的作业，DumbJob：  
```
public class DumbJob : IJob
    {
        /// <summary>
        ///  context 可以获取当前Job的各种状态。
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
 
            JobDataMap dataMap = context.JobDetail.JobDataMap;
 
            string content = dataMap.GetString("jobSays");
 
            Console.WriteLine("作业执行，jobSays:" + content);
        }
    }
```
其WithCronSchedule("") 拥有强大的Cron时间表达式，正常情况下WithSimpleSchedule(x) 已经满足大部分对日期设置的要求了。  

Quartz.Net官方2.X教程  http://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/index.html  

Quartz.Net开源地址   https://github.com/quartznet/quartznet  

作者：蘑菇先生 出处： http://mushroom.cnblogs.com/  