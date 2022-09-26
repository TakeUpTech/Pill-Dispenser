# Pill Dispenser
## Context:
As part of a school project called ‘technical project’, our goal was to design a programmable pill dispenser. It was capable of dispensing a patient’s drug treatment. To do this, an application was developed on Unity in C# in order to communicate (via cable or bluetooth) with the pill dispenser running under Arduino. The application allows you to set the frequency of treatments, to follow the drug reserves in the pill dispenser and to enable a light and/or sound alarm when taking the treatment to alert the patient. The pill dispenser was designed on 3D Experience and printed with a 3D printer.
## Features:

<p align="center">
  <img width="720" alt="pill_dispenser_app" src="https://user-images.githubusercontent.com/73184884/192194851-119b0d8e-c188-4769-a67e-69cc5139f70b.jpg">
</p>

As we can see, this application is divided into 2 parts:
- Data monitoring: this panel allows you to track the humidity and temperature for optimal pill storage (values become red if there is poor pill storage). The number intakes remaining allows to know when the change of the wheel is needed.
- Set-up: this 2 panels allow to activate or not the alarms as well as to configure the intake hours for each day (here it is every x seconds for easier experimental tests).

When the connection is established between the application and the Arduino microcontroller of the pill dispenser, it is possible to send set-up parameters to the pill dispenser. Then, it will be updated.

## State:
- [ ] Work in progress
- [X] Work completed
