﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();
        public int Count { get { return _heap.Count; } }

        public void Push(T data)
        {
            // 힙의 맨 끝에 새로운 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;

            // 도장깨기를 시작
            while (now > 0)
            {
                //나의 부모에 대한 인덱스
                int parentIndex = (now - 1) / 2;
                if (_heap[now].CompareTo(_heap[parentIndex]) < 0)
                    break;

                // 두 값을 교체한다.
                T temp = _heap[now];
                _heap[now] = _heap[parentIndex];
                _heap[parentIndex] = temp;

                // 검사 위치를 이동시킨다.
                now = parentIndex;
            }
        }

        public T Pop()
        {
            // 반환 데이터를 따로 저장.
            T ret = _heap[0];

            // 마지막 데이터를 루트로 이동한다.
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            // 양쪽을 비교하여 더 큰 숫자와 자리를 교환한다.

            // 역으로 내려가는 도장깨기 시작.
            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                // 왼쪽 값이 현재값보다 크면, 왼쪽으로 이동
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;

                // 오른 값이 현재값(왼쪽 이동 포함)보다 크면, 오른쪽으로 이동
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                // 왼쪽/오른쪽 모두 현재값보다 작으면 종료
                if (next == now)
                    break;

                // 두 값을 교체한다.
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사 위치를 이동한다
                now = next;
            }

            return ret;
        }

        public T Peek()
        {
            if (_heap.Count == 0)
                return default(T);

            return _heap[0];
        }
    }
}
