import shutil
import os
import os.path
import sys


files = r"D:\vcs\files"
patch = r"D:\vcs\patch"
vcs = r"D:\vcs\vcs.json"
restore = r"D:\RESTORE"

if sys.argv[1] == '1':
    print("删除vcs目录")
    exists = os.path.exists(files)
    if exists:
        shutil.rmtree(files)
    exists = os.path.exists(patch)
    if exists:
        shutil.rmtree(patch)
    exists = os.path.exists(vcs)
    if exists:
        os.remove(vcs)

if sys.argv[1] == '2':
    print("删除restore目录")
    exists = os.path.exists(restore)
    if exists:
        shutil.rmtree(restore)
