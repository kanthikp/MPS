# MessageProcessingSimulator ReadMe

## Overview
Application to simulate multuple types of Message generation and consumption 

### Implementation Details
* Message Publishing service creates N MessageGenerators
* Message generators create messages and inserts into Message Queue
* Message Consuming service dequeues the messages and uses MessageType specific Consumer to process
### Possible improvements
* Use Reactive Extensions to replace the Queue with Subject
