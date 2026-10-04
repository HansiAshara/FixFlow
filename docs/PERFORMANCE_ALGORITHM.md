# FixFlow - Technician Performance Algorithm Specification

## 1. Performance Feedback Loop
Technician performance is calculated exclusively from verified FixFlow work order history, preventing fabricated reviews.

---

## 2. Calculation Metrics

$$PerformanceScore = (R_{norm} \times 0.40) + (C_{rate} \times 0.30) + (Resp_{rate} \times 0.20) - Penalty_{reopen} + Baseline$$

Where:
- **$R_{norm}$ (Normalized Customer Rating)**: $\frac{AverageRating}{5.0}$ ($0.0 \dots 1.0$).
- **$C_{rate}$ (Completion Rate)**: $\frac{CompletedJobs}{AssignedJobs}$.
- **$Resp_{rate}$ (Response Rate)**: Timely acceptance/rejection within the SLA window ($24\text{ hours}$).
- **$Penalty_{reopen}$ (Resolution Reopen Penalty)**:
  $$Penalty_{reopen} = \min\left(0.20, \frac{ReopenedJobs}{TotalAssigned}\right)$$
- **$Baseline$**: $0.10$ default calibration offset.

The final score is clamped to $[0.0, 1.0]$.
