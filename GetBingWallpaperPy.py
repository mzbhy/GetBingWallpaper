#!/usr/bin/env python
# coding:utf-8

"""Download Bing everyday wallpaper.

Usage:
	GetBingWallpaperPy [-cps] [<PATH>]

Options:
	-h --help	显示帮助菜单
	-c		显示版权信息
	-p		设置壁纸前预览今日壁纸
	-s		直接设置壁纸，无需确认

Example:
	GetBingWallpaperPy
	GetBingWallpaperPy -c
	GetBingWallpaperPy -cps
	GetBingWallpaperPy -cp D:\\temp

"""

from urllib.request import urlopen, urlretrieve
from xml.dom import minidom
from PIL import Image
from docopt import docopt
import win32api, win32gui, win32con
import sys,io,os
import threading

# 改变标准输出的默认编码 
sys.stdout = io.TextIOWrapper(sys.stdout.buffer,encoding='gb18030')


class BingInfo(object):
	"""Infomation about Bing Wallpaer."""
	def __init__(self):
		super(BingInfo, self).__init__()
		_copyRight = ""
		_url = ""
		_fullDir = ""
		_saveDir = ""
		_fullDirBmp = ""

def downloadWallpaper(todayWall):
	"""Download the Bing Wallpaper using xml api."""
	try:
		wallResponse = urlopen('http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1')
		wallXML = minidom.parse(wallResponse)
		wallURL = wallXML.getElementsByTagName("url")
		todayWall._url = wallURL[0].firstChild.nodeValue
		wallCopyRight = wallXML.getElementsByTagName("copyright")
		todayWall._copyRight = wallCopyRight[0].firstChild.nodeValue
		todayWall._fullDir = todayWall._saveDir + '\\' + todayWall._url[todayWall._url.rindex("/") + 1:]
		todayWall._fullDirBmp = todayWall._fullDir.replace('jpg','bmp')	
		urlretrieve('http://cn.bing.com' + todayWall._url, todayWall._fullDir)
	except Exception as e:
		print("Image download error! Please check the internet connect!")
		return False

	try:
		Image.open(todayWall._fullDir).save(todayWall._fullDirBmp)
		os.remove(todayWall._fullDir)
		return True
	except Exception as e:
		print(e.value)
		return False
	

def setWallpaper(picPath):
	'''Set the wallpaper in Windows system.'''
	# cmd = 'REG ADD \"HKCU\Control Panel\Desktop\" /v Wallpaper /t REG_SZ /d \"%s\" /f' %picPath
	# # cmd = 'REG ADD \"HKCU\Control Panel\Desktop\" /v Wallpaper /t REG_SZ /d \" D:\1.bmp \" /f' 
	# os.system(cmd)
	# os.system('rundll32.exe user32.dll, UpdatePerUserSystemParameters')
	win32gui.SystemParametersInfo(win32con.SPI_SETDESKWALLPAPER, picPath, 1+2) 

class showImageThread(threading.Thread):
	'''Extend the threading.Thread for showing image'''
	def __init__(self, args):
		threading.Thread.__init__(self)
		self.args = args
	def run(self):
		im = Image.open(self.args[0]._fullDirBmp)
		im.show()

def main():
	'''The main function.'''
	arguments = docopt(__doc__)
	todayWall = BingInfo()

	filePath = arguments['<PATH>']
	if filePath is not None:
		todayWall._saveDir = filePath.encode('GB2312').decode('GB2312')
	else:
		todayWall._saveDir = os.path.dirname(os.path.realpath(__file__)) + "\images"

	if not os.path.exists(todayWall._saveDir):
		os.makedirs(todayWall._saveDir)
	
	if downloadWallpaper(todayWall):
		if arguments['-c']:
			print(todayWall._copyRight)
		if arguments['-p']:
			t = showImageThread(args=(todayWall,))
			t.start()
		if not arguments['-s']:
			if input('Do you want to set this image as wallpaper? [y/n]:') == 'y':
				isSet = True
			else:
				isSet = False
		else:
			isSet = True
		if isSet:
			setWallpaper(todayWall._fullDirBmp)
		else:
			print("No wallpaper is set!")
		

if __name__ == "__main__":
	main()
