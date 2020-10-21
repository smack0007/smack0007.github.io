---
Title: Code from when I was young and stupid and needed the money
Date: 2014-06-18
Tags: old-code, c++
---

We have had 2 interns at work for the past few weeks and one of the tasks we gave them was to implement [Quicksort](http://en.wikipedia.org/wiki/Quick_sort).
This got me to thinking about my youth and some of the code I wrote back then. I've decided to post it.

<!--more-->

Here is one of my old implementations of Quicksort:

```c++
void Sort(int i, int j)
{
	T temp;

	if(i<j)
	{
		if(i == j-1) // 2 items
		{
			if(m_data[i] > m_data[j])
			{
				temp = m_data[i];
				m_data[i] = m_data[j];
				m_data[j] = temp;
			}
		}
		else if(i == j-2) // 3 items
		{
			if(m_data[i] > m_data[i+1])
			{
				temp = m_data[i];
				m_data[i] = m_data[i+1];
				m_data[i+1] = temp;
			}

			if(m_data[i+1] > m_data[i+2])
			{
				temp = m_data[i+1];
				m_data[i+1] = m_data[i+2];
				m_data[i+2] = temp;
			}

			if(m_data[i] > m_data[i+1])
			{
				temp = m_data[i];
				m_data[i] = m_data[i+1];
				m_data[i+1] = temp;
			}
		}
		else if(i == j-3) // 4 items
		{
			if(m_data[i] > m_data[i+1])
			{
				temp = m_data[i];
				m_data[i] = m_data[i+1];
				m_data[i+1] = temp;
			}

			if(m_data[i+1] > m_data[i+2])
			{
				temp = m_data[i+1];
				m_data[i+1] = m_data[i+2];
				m_data[i+2] = temp;
			}
		
			if(m_data[i+2] > m_data[i+3])
			{
				temp = m_data[i+2];
				m_data[i+2] = m_data[i+3];
				m_data[i+3] = temp;
			}

			if(m_data[i] > m_data[i+1])
			{
				temp = m_data[i];
				m_data[i] = m_data[i+1];
				m_data[i+1] = temp;
			}

			if(m_data[i+1] > m_data[i+2])
			{
				temp = m_data[i+1];
				m_data[i+1] = m_data[i+2];
				m_data[i+2] = temp;
			}

			if(m_data[i] > m_data[i+1])
			{
				temp = m_data[i];
				m_data[i] = m_data[i+1];
				m_data[i+1] = temp;
			}
		}
		else // 5 or more items
		{
			int pivot = j;
			int up = i-1;
			int down = pivot;

			while(up<down)
			{
				while((m_data[++up] < m_data[pivot]) && up<down);

				if(up>=down)
					break;

				while((m_data[--down] > m_data[pivot]) && up<down);

				if(up>=down)
					break;

				temp = m_data[up];
				m_data[up] = m_data[down];
				m_data[down] = temp;
			}

			temp = m_data[up];
			m_data[up] = m_data[pivot];
			m_data[pivot] = temp;
		
			Sort(i, up-1);
			Sort(up+1, j);
		}
	}
}
```

And here is another:

```c++
template<class T>
void CArray<T>::Sort(int (*Compare)(T &item1, T &item2), int left, int right)
{
	if(left >= right)
		return;

	int up = left-1;
	int down = right;
	int pivot = m_array[right];

	if(right == left + 1)
		if(Compare(m_array[left], m_array[right]) == 1)
		{
			Swap(left, right);
			return;
		}

	while(true)
	{
		while(Compare(m_array[++up], pivot) == -1 && up<down);
		while(Compare(m_array[--down], pivot) == 1 && up<down); 
		if(up>=down) break;
		Swap(up, down);
	}

	Swap(up, right);
	Sort(Compare, left, up-1);
	Sort(Compare, up+1, right);
}
```

The first example seems to be from 2004 and the second example seems to be from 2006, though I'm not sure how
much I can actually trust the timestamps on the files. It is interesting to see how far I progressed in the
timespan of about 2 years. 2004 would have been right in the middle of college and 2006 was torwards the end
of my college time.

In the second example I think I had actually been a "professional" (aka I got paid a little money on the side) programmer
for a few months. Like I said, I was young and stupid and I needed the money.
