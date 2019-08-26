# MessageProcessingSimulator ReadMe

## Overview
Application to simulate multuple types of Message generation and consumption 

### Implementation Details
* Message Publishing service creates N MessageGenerators
* MessageGenerators create messages and inserts into Message Queue
* Message Consuming service dequeues the messages and invokes MessageType specific Consumer to process the same

### Possible improvements
* Use Reactive Extensions to replace the Queue with Subject
