using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBehaviour
{
    public abstract class BlockBehaviours : MonoBehaviour
    {
        [SerializeField] private int blockNumber;
        // Start is called before the first frame update
        public int blockNum
        {
            get { return blockNumber; }
            set { blockNumber = value; }
        }
    }
}
