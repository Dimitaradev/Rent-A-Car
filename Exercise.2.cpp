#include<iostream>;

void countNumberDigits(int number, int digitsCounterArray[], size_t size)
{
	while (number != 0)
	{
		int digit = number % 10;
		digitsCounterArray[digit]++;

		number /= 10;
	}
}

bool checkIsNumberAValidPermutation(int inputArray[], int checkedArray[], size_t size)
{
	for (int i = 0; i < size; i++)
	{
		if (inputArray[i] != checkedArray[i])
			return false;
	}

	return true;
}

void generateAllPossibleNumbers(int number)
{
	constexpr int DIGITS_COUNTER_ARRAY_SIZE = 10;

	int inputNumberDigitsCounterArray[DIGITS_COUNTER_ARRAY_SIZE] = { 0 };
	countNumberDigits(number, inputNumberDigitsCounterArray, DIGITS_COUNTER_ARRAY_SIZE);

	const int startNumber = 1000;
	const int endNumber = 9999;

	for (int i = startNumber; i <= endNumber; i++)
	{
		int currentNumberDigitsCounterArray[DIGITS_COUNTER_ARRAY_SIZE] = { 0 };
		countNumberDigits(i, currentNumberDigitsCounterArray, DIGITS_COUNTER_ARRAY_SIZE);

		bool isValidPermutation = checkIsNumberAValidPermutation(inputNumberDigitsCounterArray, currentNumberDigitsCounterArray, DIGITS_COUNTER_ARRAY_SIZE);

		if (isValidPermutation)
		{
			std::cout << i << std::endl;
		}
	}
}

int main()
{
	int number;
	std::cin >> number;

	generateAllPossibleNumbers(number);
}