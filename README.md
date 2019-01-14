# GetBingWallpaperPy
公司发的笔记本无法使用之前C#编写的工具，具体原因还待查询，为了方便使用，写了一个简单的python版本。

## 开发环境

* python 3.5.4
* sublime text 3
* windows 7 64 bit

## 使用方法

1. 安装依赖项（按需）

   ```bash
   sudo pip install docopt
   ```

2. 下载脚本

   ```bash
   git clone -b py https://github.com/lxalxy/GetBingWallpaper.git
   cd GetBingWallpaper
   ```

3. 运行脚本设置壁纸（当路径存在特殊符号以及空格时，需要在路径外加双引号）。

   ```bash
   # 设置当日Bing壁纸，壁纸存放在工作路径下的image文件夹中
   python GetBingWallpaperPy.py
   # 设置当日Bing壁纸，输出版权信息，并预览壁纸，壁纸存放在指定目录
   python GetBingWallpaperPy.py -cp D:\test
   # 设置当日Bing壁纸，无需确认，直接设置
   python GetBingWallpaperPy.py -s
   ```

## 参数说明

* -h,--help        显示帮助菜单
* -c                    显示版权信息
* -p                   设置壁纸前预览今日壁纸
* -s                    直接设置壁纸，无需确认

## 更新日志

### 2018.1.3

1. 完成初版

### 2019.1.14

1. 添加多线程处理，避免预览时终端无法操作
2. 添加直接设置壁纸参数