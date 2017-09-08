## BitmapWorkstation

### 该项目的功能是将一段内存地址数据保存到Bitmap中，技术上关注Bitmap的生成规则以及MemoryStream异常情况 
###1. BitmapHelpers生成bitmap存在两种方式
    GeneratedBitmapByBitmapImageData方式使用unsafe的方式生成bitmap，C#中直接操作指针，项目工程需要设置为不安全
    GenerateBitmapByAppendHeader方式使用安全，是给内存添加54个头信息（这54个头信息是必须的）
    bitmap生成格式是24bit的，并且按照BGR方式填充
  
###2. BitmapImage与Bitmap之间转化时候最好通过内存MemoryStream进行转换，否则容易出错。