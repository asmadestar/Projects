####################################################################################################################################
# Creating and saving grahs
# This code loads list of specified files and creates graphs 
# 'python create-plots-Part3.py <name of data> <data file1>, <name of data> <data file 2>"
# Example: 
# python create-plots-Part3.py graph1 data1 ./OverlapTest/default1-data.txt data2 ./OverlapTest/default2-data.txt
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
    sys.argv = "./OverlapTest/create-plots-Part3.py", "graph1", "data1", "./OverlapTest/default1-data.txt", "data2", "./OverlapTest/default2-data.txt"


dataBeforeTraining = np.loadtxt(sys.argv[3], comments="#", delimiter="|", unpack=False)
dataAfterTraining = np.loadtxt(sys.argv[5], comments="#", delimiter="|", unpack=False)
dataNumberOfActCols = np.loadtxt(sys.argv[7], comments="#", delimiter="|", unpack=False) 

plt.plot(dataBeforeTraining, label="Before learning")

plt.plot(dataAfterTraining, label="After learning")

plt.axvspan(0, dataNumberOfActCols, facecolor="g", alpha=0.3,
            label="Active columns")

plt.legend(loc="upper right")
plt.xlabel("Columns")
plt.ylabel("Overlap scores")
plt.title("Figure 4a-Part3: Sorted column overlaps of a SP with random "
          "input. in C#")
plt.savefig("figure_4a_Part3_C#")
plt.show()
plt.close()








