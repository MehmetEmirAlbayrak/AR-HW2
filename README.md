Objective

Load two point clouds P and Q from text files

Estimate the rigid transformation (rotation + translation) that aligns Q to P

Use 3-Point Alignment and a robust RANSAC-based method as required

Visualize the results in Unity with:

Red = P

Blue = original Q

Green = aligned Q'

Display the transformation parameters and inlier count

Provide UI buttons to select the registration method and visualization mode

 What I Implemented

A file loader that reads point clouds in the homework format (x, y, z rows).

Visualization of P, Q and aligned Q' using colored sphere markers.

3-Point Alignment to compute the basic rigid transform.

RANSAC + 3-Point Alignment to handle outliers and find the model with maximum inliers.

A second visualization mode showing motion lines from each Q point to its aligned Q'.

A simple UI with buttons:

Load & Show

3-Point Only

RANSAC

Toggle View

On-screen display of:

Rotation matrix R

Translation vector T

*Scale*

Inliers / total points

 **Summary**

This project fulfills all homework requirements:
loading point sets, implementing two registration methods, visualizing original and aligned points, showing movement lines, and displaying transformation parameters inside a functional Unity interface.
