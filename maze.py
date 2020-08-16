from pybrain.tools.shortcuts import buildNetwork
from pybrain.datasets import SupervisedDataSet
from pybrain.supervised.trainers import BackpropTrainer
from PIL import Image

im = Image.open('C:/maze.png')
pix = im.load()

maze = []
for i in range(im.size[0]):
	maze.append([])
	for j in range(im.size[1]):
		if pix[j,i] == (0,0,0) or pix[j,i] == (1,1,1):
			maze[i].append(1)
		elif pix[j,i] == (255, 255, 255) or pix[j,i] == (254, 254, 254):
			maze[i].append(0)

inputs = [[1,1,1,0,0,0,0,1],[1,1,0,1,0,0,1,0],[0,1,1,1,1,0,0,0],[1,0,1,1,0,1,0,0],[1,0,0,1,0,1,0,0],[1,0,0,1,0,0,1,0],[0,1,1,0,1,0,0,0],[0,1,1,0,0,0,0,1],[0,0,1,1,0,1,0,0],[0,0,1,1,1,0,0,0],[0,1,0,1,0,0,1,0],[0,1,0,1,1,0,0,0],[1,1,0,0,0,0,0,1],[1,1,0,0,0,0,1,0],[1,0,1,0,0,1,0,0],[1,0,1,0,0,0,0,1],[0,0,0,1,1,0,0,0],[0,0,0,1,0,0,1,0],[0,0,0,1,0,1,0,0],[0,0,1,0,0,1,0,0],[0,0,1,0,1,0,0,0],[0,0,1,0,0,0,0,1],[1,0,0,0,0,1,0,0],[1,0,0,0,0,0,0,1],[1,0,0,0,0,0,1,0],[0,1,0,0,0,0,1,0],[0,1,0,0,1,0,0,0],[0,1,0,0,0,0,0,1]]
outputs = [[0,0,0,1],[0,0,1,0],[1,0,0,0],[0,1,0,0],[0,0,1,0],[0,1,0,0],[0,0,0,1],[1,0,0,0],[1,0,0,0],[0,1,0,0],[1,0,0,0],[0,0,1,0],[0,0,1,0],[0,0,0,1],[0,0,0,1],[0,1,0,0],[0,1,0,0],[1,0,0,0],[0,0,1,0],[0,0,0,1],[0,1,0,0],[0,1,0,0],[0,0,0,1],[0,0,1,0],[0,0,0,1],[0,0,0,1],[0,0,1,0],[1,0,0,0]]

net = buildNetwork(8,16,4, bias = True)
ds = SupervisedDataSet(8,4)
coords = []

for i, j in zip(inputs, outputs):
	ds.addSample(tuple(i), tuple(j))

trainer = BackpropTrainer(net, ds)
for i in range(100):
	trainer.train()

for i in range(im.size[0]):
	if maze[0][i] == 0:
		posX = 0
		posY = i

coords.append([posX,posY])
maze[posX][posY] = 2
posX = posX + 1

for i in range(im.size[0]):
	if maze[im.size[1] - 1][i] == 0:
		posendX = im.size[1]
		posendY = i

coords.append([posX,posY])

step = []
while posX != posendX - 1:
	if maze[posX - 1][posY] != 1:
		step.append(0)
	else: step.append(1)
	if maze[posX][posY - 1] != 1:
		step.append(0)
	else: step.append(1)
	if maze[posX][posY + 1] != 1:
		step.append(0)
	else: step.append(1)
	if maze[posX + 1][posY] != 1:
		step.append(0)
	else: step.append(1)

	if maze[posX - 1][posY] == 2:
		step.append(1)
	else: step.append(0)
	if maze[posX][posY - 1] == 2:
		step.append(1)
	else: step.append(0)
	if maze[posX][posY + 1] == 2:
		step.append(1)
	else: step.append(0)
	if maze[posX + 1][posY] == 2:
		step.append(1)
	else: step.append(0)

	compute = net.activate(step)

	for i in range(im.size[0]):
		for j in range(im.size[1]):
			if maze[i][j] == 2:
				maze[i][j] = 0

	maze[posX][posY] = 2

	for i in range(len(compute)):
		if compute[i] == max(compute):
			if i == 0:
				posX -= 1 
			elif  i == 1:
				posY -= 1 
			elif  i == 2:
				posY += 1
			elif  i == 3:
				posX += 1 

	coords.append([posX, posY])
	step.clear()

for i in range(im.size[0]):
		for j in range(im.size[1]):
			if maze[i][j] == 2:
				maze[i][j] = 0

maze[posX][posY] = 2
coords.append([posX, posY])

file = open('C:/text.txt', 'w')
for i in coords:
    print(i, file= file)
