# APManagerC2

全名：Account&Password Manager C2

原本是弄来储存各种网站帐号用的小软件，后来发现用来储存其他信息也是挺好的，处于显而易见的原因考虑，没有修改名字（

这个是第二版



## 基本介绍：

**用户内容包括头像、用户名、用户密码和用户简述**

用户密码用于加密数据库文件，用户名和用户简述则是简单的个性化设置，用户头像将作为容器的默认头像

每次设置内容或者保存修改时，将会将数据库重新用用户密码进行加密，因此密码为核心设置

密码对比方式为对比密码的Hash值

* 注1：默认用户名为AAA，默认密码为000000

* 注2：即使不输入密码也可以登录到主界面，但是将不会加载任何数据，且大多数操作将不可用

* 注3：登陆界面左上角可以导出当前用户的相关数据内容至压缩包



**数据内容主要以主要由筛选器与容器组成，筛选器用于对容器进行分类整理，而容器则是实际储存数据的地方**

**主界面的左侧部分为筛选器**

1. 作用：

   过滤容器列表，关闭的筛选器所属的容器将不会显示在右侧容器列表区

2. 可用操作：

   添加、移除、修改筛选器，开/关状态

**主界面右侧部分为容器**

 1. 作用

    实际储存数据记录的地方，点击标签即可查看内容

	2. 可用操作

    添加、移除、修改，设置容器模板

    * 容器模板于主界面右下角中的设置区可以找到，当添加容器的时候，会以模板的内容新建数据



## 快捷键：

主界面：

* <kbd>F1</kbd>：添加一个过滤器

* <kbd>F2</kbd>：添加一个容器
* <kbd>F5</kbd>：重新加载数据库
* <kbd>Ctrl</kbd>+ <kbd>S</kbd>：保存

容器：

* <kbd>F1</kbd>：添加一个数据记录
* <kbd>Esc</kbd>：关闭
* <kbd>Ctrl</kbd> +<kbd>S</kbd>：保存