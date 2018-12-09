# CoolQ-Torrent-Seacher
## 该插件改写自https://github.com/Anliex/com.anliex.Search
## 使用的SDK：https://cqp.cc/forum.php?mod=viewthread&tid=28865&highlight=flexlive

<br>
酷Q用磁力链搜索插件。<br>
使用的搜索引擎是bturl.cc，所以使用时要先确保能上这个网站。<br>
礼节性感谢一下原作者。<br>
不过原作者的代码只有搜索一条的功能。github上的这个甚至连标题都没法显示，所以我二次开发了一下。<br>
现在可设置管理员QQ，可控制任意一个群是否开启搜索（因为管理员得在群里喊，所以只有机器人在，管理员不在的群，默认是关的，而且改不了）<br>
除此之外，还能指令控制搜索条数。这个指令谁都能用。<br>
然后，毕竟会搜出一些神奇的词汇，所以具体信息不会在群里显示，私聊机器人能看到标题、上传时间、文件大小、下载热度以及磁力链。<br>

### 使用方法：
1.public文件夹内的东西放到酷Q根目录里<br>
2.重启酷Q<br>
3.启用Flexlive.CQP.CQEProxy（第二个E语言版）插件<br>
4.菜单中打开CSharp代理程序<br>
5.启动磁力搜索插件（没的话重新加载应用试试）<br>
6.设置中设置管理员QQ<br>
7.具体指令可见插件说明<br>

### 开发：
VS开解决方案就行。里面那个压缩包就是原版SDK，里面有帮助说明，可以参考。
PS:public/CSharpPlugins内的.dll才是插件本体
