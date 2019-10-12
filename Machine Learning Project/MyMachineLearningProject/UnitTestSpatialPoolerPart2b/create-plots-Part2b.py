####################################################################################################################################
# Creating and saving grahs
# This code loads list of specified files and creates line graphs 
# 'python create-plots-Part3.py <name of data> <data file1>, <name of data> <data file 2>"
# Example: 
# python create-plots-Part2b.py graph1 data1 ./OverlapTest/default1-data.txt data2 ./OverlapTest/default2-data.txt
####################################################################################################################################

import plotly
import csv
import sys
import numpy as np
import plotly.plotly as py
import plotly.tools as tls
import matplotlib.pyplot as plt

if len(sys.argv) <= 2:
    print("WARNING: Start with argumnet. I.E.: 'python create-histogram.py 28, data1, .\dataFile1.txt, data2, .\dataFile2.txt'")
    print("'python create-histogram.py <title>, <name of data> <data file1>,.., <name of data> <data file N>")
    sys.argv = "./OverlapTest/create-plots-Part2b.py", "graph1", "data1", "./OverlapTest/default1-data.txt", "data2", "./OverlapTest/default2-data.txt"

dataX = np.loadtxt(sys.argv[3], comments="#", delimiter="|", unpack=False)
dataY = np.loadtxt(sys.argv[5], comments="#", delimiter="|", unpack=False)

plt.plot(dataX,dataY)

plt.legend(loc='upper right')
plt.xlabel("Input overlap")
plt.ylabel("Output overlap")
title("Figure 3-Part2b Output overlap in function of input one in a SP in C# without training")

plt.savefig("figure3_Part2b_In_C#")
plt.show()
plt.close()


