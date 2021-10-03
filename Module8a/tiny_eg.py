np.random.seed(0)


def spiral_data(points, classes):
    X = np.zeros((points*classes, 2))
    y = np.zeros(points*classes, dtype='uint8')
    for class_number in range(classes):
        ix = range(points*class_number, points*(class_number+1))
        r = np.linspace(0.0, 1, points)  # radius
        t = np.linspace(class_number*4, (class_number+1)*4, points) + np.random.randn(points)*0.2
        X[ix] = np.c_[r*np.sin(t*2.5), r*np.cos(t*2.5)]
        y[ix] = class_number
    return X, y


#nnfs.init()

X, y = spiral_data(100, 3)

# -----
# ACTIVATION FUNCTIONS

class Activation_ReLU:
    def forward(self, inputs):
        self.output = np.maximum(0, inputs)

# -----
# LAYER FUNCTIONS
class Layer_Dense:
    def __init__(self, n_inputs, n_neurons):
        self.weights = 0.10 * np.random.randn(n_inputs, n_neurons)
        self.biases = np.zeros((1, n_neurons))
    # Dot product + biases for this layer
    def forward(self, inputs):
        self.output = np.dot(inputs, self.weights) + self.biases



layer1 = Layer_Dense(2,5)
layer1.forward(X)

activation1 = Activation_ReLU()
activation1.forward(layer1.output)

print(activation1.output)