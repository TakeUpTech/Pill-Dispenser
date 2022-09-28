# Pill Dispenser
## Context:
As part of a school project called ‘technical project’, our goal was to design a programmable pill dispenser. It was capable of dispensing a patient’s drug treatment. To do this, an application was developed on Unity in C# in order to communicate (via cable or bluetooth) with the pill dispenser running under Arduino. The application allows you to set the frequency of treatments, to follow the drug reserves in the pill dispenser and to enable a light and/or sound alarm when taking the treatment to alert the patient. The pill dispenser was designed on 3D Experience and printed with a 3D printer.

## Conception:
We made our product from scratch. We first thought about the different features of the pill dispenser as well as the technical solutions that would satisfy them. Then, we dimensioned our pill dispenser in order to model it in 3D using 3D Experience. The product was then printed with a 3D printer. In parallel, the application as well as the electrical circuit of the pill dispenser was realized. The source codes of the pillbox and the application are also available in this repository. Here is a preview of the pillbox on 3D Experience and its electric circuit:

<p align="center">
  <img width="720" alt="pill_dispenser_3d" src="https://user-images.githubusercontent.com/73184884/192203932-5c512cc7-36a5-49a6-bf13-b65be91ccfda.png">
</p>

<p align="center">
  <img width="720" alt="electric_circuit" src="https://user-images.githubusercontent.com/73184884/192292360-7244d05f-bba9-489b-8269-bbe5ee175003.jpg">
</p>

## Features:
Here is an overview of the interface of the application coded on Unity:
<p align="center">
  <img width="720" alt="pill_dispenser_app" src="https://user-images.githubusercontent.com/73184884/192194851-119b0d8e-c188-4769-a67e-69cc5139f70b.jpg">
</p>

As we can see, this application is divided into 2 parts:
- Data monitoring: this panel allows you to track the humidity and temperature for optimal pill storage (values become red if there is poor pill storage). The number intakes remaining allows to know when the change of the wheel is needed.
- Set-up: this 2 panels allow to activate or not the alarms as well as to configure the intake hours for each day.
> Here it is every x seconds for easier experimental tests.

When the connection is established between the application and the Arduino microcontroller of the pill dispenser, it is possible to send set-up parameters to the pill dispenser. Then, it will be updated.

## State:
- [ ] Work in progress
- [X] Work completed
