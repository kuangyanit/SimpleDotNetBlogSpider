# 老赵博客爬虫

## A.实现细则
- 利用WebClient对象下载相应的html网页，比如[这个](http://blog.zhaojie.me/?page=1)
- 利用NSoup解析下载的html网页
- 将需要的信息存储到文本文件中