// TestNN.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream> // cout ect..


#include <variant> // HMM

#include <string>
#include <fstream>
#include <vector>
#include <utility> // std::pair

#include <stdexcept> // std::runtime_error
#include <sstream> // std::stringstream

#include <algorithm>    // std::max

#include <Eigen/Dense>

//#include <stdlib.h>     /* srand, rand */

template<typename  T>
using Column = std::vector<T>;


class FileHandler
{
private:
	std::string fileName_;

	//std::fstream file_;


public:

	FileHandler(std::string fileName) {
		fileName_ = fileName;
	}

	template<typename T>
	void writeToFile(std::string filename, std::vector<std::vector<T>> dataset) {


		// Create an output filestream
		std::ofstream writeFile(filename);


		// Make sure the file is open
		if (!writeFile.is_open()) throw std::runtime_error("Could not open file");


		for (int i = 0; i < dataset[0].size(); ++i)
		{
			for (int j = 0; j < dataset.size(); ++j)
			{
				//std::cout << i << j;
				writeFile << dataset[j][i];
				if (j != dataset.size() - 1) writeFile << ","; // No comma at end of line
			}
			writeFile << "\n";
		}

		// Close
		writeFile.close();
	}

	template <typename T>
	void readFromFile(std::string filename, std::vector<std::vector<T>>& result ) {//, T type
		// Reads a file into a vector of <string, vector<string>> pairs where
		// each pair represents <column name, column values>

		// Create an input filestream
		std::ifstream readFile(filename);

		// Make sure the file is open
		if (!readFile.is_open()) throw std::runtime_error("Could not open file");

		// Helper vars
		std::string line, colname;
		T val;

		// Read the column names
		if (readFile.good())
		{
			// Extract the first line in the file
			std::getline(readFile, line);

			// Create a stringstream from line
			std::stringstream ss(line);

			int colIdx = 0;//decltype

			// Extract each string
			while (ss >> val) {

				result.push_back(std::vector<T> {});

				result[colIdx].push_back(val);

				// If the next token is a comma, ignore it and move on
				if (ss.peek() == ',') ss.ignore();

				// Increment the column index
				colIdx++;
			}

			//std::cout << result.size();
		}

		//std::cout << result.size();

		// Read data, line by line
		while (std::getline(readFile, line))
		{
			// Create a stringstream of the current line
			std::stringstream ss(line);

			// Keep track of the current column index
			int colIdx = 0;

			//result.push_back(std::vector<T> {});

			// Extract each string
			while (ss >> val) {
				//std::cout << val;
				//if (colIdx != 1)
				result[colIdx].push_back(val);

				// If the next token is a comma, ignore it and move on
				if (ss.peek() == ',') ss.ignore();

				// Increment the column index
				colIdx++;
			}
		}

		// Close
		readFile.close();
	}

	void readFromFile(std::string filename, std::vector<std::vector<std::string>>& result) {//, T type
		// Reads a file into a vector of <string, vector<string>> pairs where
		// each pair represents <column name, column values>

		// Create an input filestream
		std::ifstream readFile(filename);

		// Make sure the file is open
		if (!readFile.is_open()) throw std::runtime_error("Could not open file");


		// Helper vars
		std::string line;
		std::string val;


		// Read the column names
		if (readFile.good())
		{
			// Extract the first line in the file
			std::getline(readFile, line);

			// Create a stringstream from line
			std::stringstream ss(line);

			int colIdx = 0;

			while (std::getline(ss, val, ',')) {
				result.push_back(std::vector<std::string> {});
				//std::cout << val;
				//std::cout << "\n";
				result[colIdx].push_back(val);
				colIdx++;
			}

		}

		// Read data, line by line
		while (std::getline(readFile, line))
		{
			// Create a stringstream of the current line
			std::stringstream ss(line);

			// Keep track of the current column index
			int colIdx = 0;

			// Extract each string
			while (std::getline(ss, val, ',')) {

				result[colIdx].push_back(val);

				// Increment the column index
				colIdx++;
			}
		}

		// Close
		readFile.close();
	}


};

void testFileData()
{
	std::cout << "Test File write \n";

	std::vector<int> vec1(5, 1);
	std::vector<int> vec2(5, 2);
	std::vector<int> vec3(5, 3);

	std::vector<std::vector<int>> testWriteData = { vec1,vec2,vec3 };



	FileHandler testData("test.txt");
	testData.writeToFile("test.txt", testWriteData);


	std::cout << "Test File read \n";

	std::vector<std::vector<std::string>> testReadData;

	testData.readFromFile("test.txt", testReadData);

	testData.writeToFile("test2.txt", testReadData);
}




void printMatrix(std::vector<std::vector<double>>& matrix)
{
	for (int i = 0; i < matrix.size(); i++) {
		for (int j = 0; j < matrix[0].size(); j++) {
			std::cout << matrix[i][j];
			std::cout << " ";
		}
		std::cout << "\n";
	}
}

std::vector<std::vector<double>> sigmoid(std::vector<std::vector<double>> t)
{
	std::vector<std::vector<double>> result;
	for (int i = 0; i < t.size(); i++) {
		result.push_back(std::vector<double>{});
		for (int j = 0; j < t[i].size(); j++) {
			result[i].push_back(0.0);
		}
	}

	for (int i = 0; i < t.size(); i++) {
		for (int j = 0; j < t[i].size(); j++) {
			result[i][j] = 1 / (1 + exp(-t[i][j]));
		}
	}
	//return 1 / (1 + exp(-t));
	return result;
}

std::vector<std::vector<double>> sigmoid_derivative(std::vector<std::vector<double>> p)
{
	std::vector<std::vector<double>> result;
	for (int i = 0; i < p.size(); i++) {
		result.push_back(std::vector<double>{});
		for (int j = 0; j < p[i].size(); j++) {
			result[i].push_back(0.0);
		}
	}

	for (int i = 0; i < p.size(); i++) {
		for (int j = 0; j < p[i].size(); j++) {
			result[i][j] = p[i][j] * (1 - p[i][j]);
		}
	}
	//return p * (1 - p);
	return result;
}

std::vector<std::vector<double>> transpose(std::vector<std::vector<double>> t)
{
	std::vector<std::vector<double>> result;

	int i, j;
	for (i = 0; i < t[0].size(); ++i) {
		result.push_back(std::vector<double>{});
		for (j = 0; j < t.size(); ++j) {
			result[i].push_back(0.0);
		}
	}

	//printMatrix(result);
	//printMatrix(t);

	for (i = 0; i < t.size(); ++i) {
		for (j = 0; j < t[0].size(); ++j) {
			result[j][i] = t[i][j];
		}
	}

	return result;
}


//int LOL = 0;

std::vector<std::vector<double>> dot_product(std::vector<std::vector<double>> a, std::vector<std::vector<double>> b)
{
	//LOL += 1;

	//std::cout << "\n ---- LOL: " << LOL << " ----- \n";

	//std::cout << "\na----\n";
	//printMatrix(a);
	//std::cout << "b----\n";
	//printMatrix(b);


	int i_size = a.size();
	int j_size = b[0].size();
	int k_size = b.size();

	//std::cout << "(" << a.size() << ", " << a[0].size() << ")" << "(" << b.size() << ", " << b[0].size() << ")" << "\n";

	i_size = a.size();
	j_size = b[0].size();
	k_size = a[0].size();


	std::vector<std::vector<double>> c;
	for (int i = 0; i < a.size(); i++) {
		c.push_back(std::vector<double>{});
		for (int j = 0; j < b[0].size(); j++) {
			c[i].push_back(0.0);
		}
	}
	//std::cout << "c----\n";
	//printMatrix(c);

	//std::cout << a.size() << "\n";
	//std::cout << b[0].size() << "\n";
	//std::cout << b.size() << "\n";

	for (int i = 0; i < i_size; i++) {
		for (int j = 0; j < j_size; j++) {
			for (int k = 0; k < k_size; k++) {
				//std::cout << i << " - " << j << " - " << k  << " - \n";
				//std::cout << a[i][k] << " - " << b[k][j] << " - \n";
				//std::cout << a[i][k];
				//std::cout << b[k][j];
				//a[i][k] * b[k][j];
				//std::cout << a[i][k] << " ";
				c[i][j] += a[i][k] * b[k][j];
			}
			//std::cout << "\n";
		}
	}

	//std::cout << "c-----\n";
	//printMatrix(c);

	return c;
}

std::vector<std::vector<double>> multiply(std::vector<std::vector<double>> matrix, double multiplyer)
{
	std::vector<std::vector<double>> result;
	for (int i = 0; i < matrix.size(); i++) {
		result.push_back(std::vector<double>{});
		for (int j = 0; j < matrix[i].size(); j++) {
			result[i].push_back(0.0);
		}
	}

	for (int i = 0; i < matrix.size(); i++) {
		for (int j = 0; j < matrix[0].size(); j++) {
			result[i][j] = matrix[i][j] * multiplyer;
		}
	}

	return result;
}


std::vector<std::vector<double>> multiply(std::vector<std::vector<double>> a, std::vector<std::vector<double>> b)
{
	std::vector<std::vector<double>> result;
	for (int i = 0; i < a.size(); i++) {
		result.push_back(std::vector<double>{});
		for (int j = 0; j < a[i].size(); j++) {
			result[i].push_back(0.0);
		}
	}

	for (int i = 0; i < a.size(); i++) {
		for (int j = 0; j < a[0].size(); j++) {
			result[i][j] = a[i][j] * b[i][j];
		}
	}

	return result;
}

std::vector<std::vector<double>> add_subtract(std::vector<std::vector<double>> a, std::vector<std::vector<double>> b, double sign)
{
	std::vector<std::vector<double>> result;

	for (int i = 0; i < a.size(); i++) {
		result.push_back(std::vector<double>{});
		for (int j = 0; j < a[i].size(); j++) {
			result[i].push_back(0.0);
		}
	}

	for (int i = 0; i < a.size(); i++) {
		for (int j = 0; j < a[0].size(); j++) {
			result[i][j] = a[i][j] + (sign * b[i][j]);
		}
	}
	return result;
}


class testNeuralNetwork
{
private:
public:
	std::vector<std::vector<double>> input; // INPUT // The tested agains values // Used to predict
	std::vector<std::vector<double>> y; // ACTUAL OUTPUT // The correct values for the input(x) values // Used as reference for training

	std::vector<std::vector<double>> weights1; // INPUT -> weights1 -> HIDDEN (3*4)
	std::vector<std::vector<double>> weights2; // HIDDEN -> weights2 -> OUTPUT (1*4)

	std::vector<std::vector<double>> output; // PREDICTED OUTPUT // The values that the network pridicts


	std::vector<std::vector<double>> layer1;
	std::vector<std::vector<double>> layer2;

	testNeuralNetwork(std::vector<std::vector<double>> X, std::vector<std::vector<double>> Y)
	{
		input = X;
		y = Y;

		//weights1 = {
		//	{0.08254769, 0.61080252, 0.65574446, 0.61899502},
		//	{0.47348028, 0.16367913, 0.93356017, 0.52779567},
		//	{0.09299463, 0.45516546, 0.34760086, 0.74777724}
		//};

		std::vector<std::vector<double>> weights1_;

		int number_feature =  X[0].size();

		int number_nodes = 4;//568;

		for (int i = 0; i < number_feature; i++) {
			weights1_.push_back(std::vector<double>{});
			for (int j = 0; j < number_nodes; j++) {
				weights1_[i].push_back(((double)rand() / (RAND_MAX + 1)));
			}
		}

		//printMatrix(weights1_);

		weights1 = weights1_;

		std::vector<std::vector<double>> weights2_;

		for (int j = 0; j < number_nodes; j++) {
			weights2_.push_back(std::vector<double>{});
			weights2_[j].push_back(((double)rand() / (RAND_MAX + 1)));
		}

		weights2 = weights2_;

		//weights2 = {
		//	{0.1},
		//	{0.37},
		//	{0.86},
		//	{0.01}
		//};


		std::vector<std::vector<double>> output_;

		for (int i = 0; i < y.size(); i++) {
			output_.push_back(std::vector<double>{});
			for (int j = 0; j < y[i].size(); j++) {
				output_[i].push_back(0.0);
			}
		}

		output = output_;

		//output = {
		//	{0},
		//	{0},
		//	{0},
		//	{0}
		//};
	}

	std::vector<std::vector<double>> feedForward()
	{
		//std::cout << "---1---\n";
		/*std::cout << "\na----\n";
		printMatrix(input);

		std::cout << "b----\n";
		printMatrix(weights1);

		std::cout << "lel----\n";*/
		//std::vector<std::vector<double>> lel = dot_product(input, weights1);
		//printMatrix(lel);

		std::vector<std::vector<double>> ly1 = sigmoid(dot_product(input, weights1));

		//sigmoid(ly1);
		//layer1 = ly1;
		std::vector<std::vector<double>> ly2 = sigmoid(dot_product(ly1, weights2));
		//sigmoid(ly2);
		layer1 = ly1;
		layer2 = ly2;
		//std::cout << "---2---\n";
		return ly2;
	}

	void backProp()
	{
		//std::cout << "---3---\n";
		std::vector<std::vector<double>> ly1 = transpose(layer1);

		//std::cout << "---4---\n";

		std::vector<std::vector<double>> bp1 = add_subtract(y, output, -1);
		bp1 = multiply(bp1, sigmoid_derivative(output));
		//std::cout << "---5---\n";
		bp1 = multiply(bp1, 2);

		//std::cout << "---6---\n";

		//std::cout << "(" << ly1.size() << ", " << ly1[0].size() << ")" << "(" << bp1.size() << ", " << bp1[0].size() << ")" << "\n";

		//printMatrix(ly1);
		//printMatrix(bp1);

		std::vector<std::vector<double>> d_wghts2 = dot_product(ly1, bp1);

		//std::cout << "---7---\n";

		std::vector<std::vector<double>> d_wghts1 = dot_product(transpose(input), multiply( dot_product(multiply(multiply(add_subtract(y,output,-1),2), sigmoid_derivative(output)), transpose(weights2)) , sigmoid_derivative(layer1)));

		//std::cout << "\n ------ AGHHHH ---- \n";
		//printMatrix(d_wghts1);

		//weights2 = d_wghts2;
		//weights1 = d_wghts1;

		//std::cout << "---8---\n";

		weights2 = add_subtract(weights2,d_wghts2,1);
		weights1 = add_subtract(weights1,d_wghts1,1);

		//std::cout << "---9---\n";

	}

	void train(std::vector<std::vector<double>> X, std::vector<std::vector<double>> Y)
	{

		//std::cout << "feedForward";
		//std::cout << "---ff---\n";
		output = feedForward();
		//std::cout << "backprop";
		//std::cout << "---bp---\n";
		backProp();
	}


};



int main()
{

	//testFileData();

	std::cout << "NOW FOR IRIS\n";

	FileHandler testFile("data.csv");

	////std::string outData = testFile.readFromFile();



	//std::vector<std::vector<double>> irisValues_;

	//testFile.readFromFile("data.csv", irisValues_);

	//std::vector<std::vector<double>> irisValues;

	//std::cout << "\nIris values columns: " << irisValues_.size();

	//printMatrix(irisValues_);

	std::vector<std::vector<std::string>> irisString;

	testFile.readFromFile("data.csv", irisString);
	////std::cout << stod(irisString[0][0]);


	std::vector<std::vector<double>> irisValues;


	for (int i = 2; i < irisString.size(); i++) {
		irisValues.push_back(std::vector<double>{});
		for (int j = 1; j < irisString[0].size(); j++) {
			//std::cout << std::stod(irisString[i][j]);
			irisValues[i-2].push_back(std::stod(irisString[i][j]));
		}
	}

	std::cout << "\nIris values columns: " << irisValues.size();

	//std::vector<std::string> irisClasses = irisString[4];

	//"id","diagnosis","radius_mean","texture_mean","perimeter_mean","area_mean","smoothness_mean","compactness_mean","concavity_mean","concave points_mean","symmetry_mean","fractal_dimension_mean","radius_se","texture_se","perimeter_se","area_se","smoothness_se","compactness_se","concavity_se","concave points_se","symmetry_se","fractal_dimension_se","radius_worst","texture_worst","perimeter_worst","area_worst","smoothness_worst","compactness_worst","concavity_worst","concave points_worst","symmetry_worst","fractal_dimension_worst",

	//std::cout << irisClasses[1];

	std::vector<std::vector<double>> irisClasses;

	//irisClasses.push_back(std::vector<double>{});
	//for (int i = 0; i < irisString[4].size(); i++) {
	//	if (irisString[4][i] == "Iris-setosa") {
	//		irisClasses[0].push_back(0);
	//	}
	//	if (irisString[4][i] == "Iris-versicolor") {
	//		irisClasses[0].push_back(1);
	//	}
	//	if (irisString[4][i] == "Iris-virginica") {
	//		irisClasses[0].push_back(2);
	//	}
	//}

	irisClasses.push_back(std::vector<double>{});
	for (int i = 0; i < irisString[1].size(); i++) {
		if (irisString[1][i] == "M")
			irisClasses[0].push_back(1);
		if (irisString[1][i] == "B")
			irisClasses[0].push_back(0);
	}

	std::cout << "\nIris class types columns: " << irisClasses.size();
	std::cout << "\n";




	// -----


	std::cout << "\n\n\n";

	//std::vector<std::vector<double>> matrix_;


	//matrix_ = {
	//	{1, 5, 6, 23},
	//	{62, 75, 3, 12},
	//	{83, 2, 43, 123},
	//	{26, 26, 82, 43}
	//};

	FileHandler iris_2("iris.data");

	std::vector<std::vector<double>> iris_data_values;
	iris_2.readFromFile("iris.data", iris_data_values);
	std::vector<double> class_column = iris_data_values.back();
	iris_data_values.pop_back();
	
	std::vector<std::vector<double>> iris_classes;
	iris_classes.push_back(class_column);






	std::vector<std::vector<double>> matrix_x;
	std::vector<std::vector<double>> matrix_y;

	matrix_x = {
		{0, 0, 1, 0},
		{0, 1, 1, 1},
		{1, 0, 1, 1},
		{1, 1, 1, 0},
	};

	matrix_y = {
		{0},
		{1},
		{1},
		{0},
	};


	matrix_x = irisValues;
	matrix_y = irisClasses;

	std::cout << "OG Matrix \n";

	matrix_x = iris_data_values;
	matrix_y = iris_classes;

	matrix_x = transpose(matrix_x);
	matrix_y = transpose(matrix_y);

	std::cout << "(" << matrix_x.size() << ", " << matrix_x[0].size() << ")" << "(" << matrix_y.size() << ", " << matrix_y[0].size() << ")" << "\n";

	std::cout << matrix_x.size();


	//printMatrix(matrix_x);
	//printMatrix(matrix_y);

	//std::vector<std::vector<double>> matrix_t = transpose(matrix_x);
	//printMatrix(matrix_t);


	//return 0;


	std::cout << "-------NN------- \n\n\n";

	testNeuralNetwork NN(matrix_x, matrix_y);

	for (int i = 0; i < 1; i++)
	{
		//break;
		if (i % 100 == 0)//i % 100 == 0
		{
			std::cout << "- iteration - " << i << "\n";

			std::cout << "- Input:" << "\n";
			//printMatrix(NN.input);

			std::cout << "- Actual Output:" << "\n";
			//printMatrix(NN.y);

			std::cout << "- Predicted Output:" << "\n";
			std::vector<std::vector<double>> ff = NN.feedForward();
			//printMatrix(ff);

		}
		NN.train(NN.input, NN.y);
		std::cout << "- iteration - " << i << "\n";
	}

	std::cout << "- Input:" << "\n";
	//printMatrix(NN.input);

	std::cout << "- Actual Output:" << "\n";
	//printMatrix(NN.y);

	std::cout << "- Predicted Output:" << "\n";
	std::vector<std::vector<double>> ff = NN.feedForward();
	//printMatrix(ff);

	for (int i = 0; i < NN.y.size(); i++) {
		for (int j = 0; j < ff[0].size(); j++) {
			std::cout << "Actual: " << NN.y[i][j] << " Predicted: " << ff[i][j] << "\n";
		}
	}

	//testFileData();


	//std::cout << "T Matrix \n";

	//std::vector<std::vector<double>> matrix_t = transpose(matrix_);

	//printMatrix(matrix_t);

}


