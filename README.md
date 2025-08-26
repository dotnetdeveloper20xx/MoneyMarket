
# MoneyMarket Platform

The platform is an intuitive, user-friendly application that connects individuals seeking loans with lenders ready to invest. It's a two-sided marketplace designed to make the borrowing and lending process simple, transparent, and mutually beneficial. Whether you need a loan or want to earn a return on your money, this app has a solution for you. 

---

## For Borrowers: Get the Money You Need

### Simple Access to Funds
As a borrower, you can easily register and apply for a loan that fits your needs. You'll see a variety of loan products from different lenders, each with its own terms, interest rates, and repayment options. This gives you the power to compare and choose the best offer for your situation, whether it's for a short, mid, or long-term need. The application guides you through the process, from providing your personal and financial details to submitting your application.

### Manage Your Loan with Ease
Once your loan is approved and funded, you can manage everything directly from your personal dashboard. Your dashboard gives you a clear overview of all your loans, their status, and a breakdown of your repayments. You'll always know how much you've borrowed, how much you've paid back, and what's left to pay. When it's time to make a payment, the app makes it simple, showing you exactly what's due and allowing you to make payments effortlessly.

### Stay Informed and Connected
The app keeps you in the loop with real-time notifications about key events. You'll receive alerts when your loan is approved, when a lender funds your loan, or when a payment is overdue. If you have questions, you can use the built-in messaging feature to communicate directly with your lender or the support team. Your profile page allows you to easily update your contact information and details.

---

## For Lenders: Invest and Earn

### Find Opportunities to Lend
As a lender, you can register and create your own loan products, setting your own terms, amounts, and interest rates to attract borrowers. The platform allows you to compete with other lenders to fund loans, giving you an active role in growing your portfolio. You'll have access to a pool of borrowers seeking funds, presenting you with a continuous flow of potential investments.

### Track Your Investments
Your dashboard is your command center, offering a complete summary of your lending activity. You can see your total investment, your total profit, and a list of all the loans you've funded. The app makes it easy to track payments, showing you which borrowers have paid and which payments are overdue. This transparency allows you to stay on top of your investments and make informed decisions.

### Secure and Profitable
The platform is designed to help you generate a profit. For every loan you fund, you'll earn a percentage of the interest paid by the borrower. The app handles all the administrative work, from disbursing funds to managing repayments, so you can focus on growing your portfolio. Just like borrowers, you'll receive notifications about important events, such as when a borrower makes a repayment or when a new borrower signs up. You can also communicate with borrowers or the support team through the messaging feature.



The application has been designed with **Clean Architecture, CQRS, Domain-Driven Design (DDD)** principles, ensuring a scalable, maintainable, and secure financial platform.  

This document explains how the application works, the roles involved, what data is collected, how loans function as first-class entities, and how we ensure compliance, security, and auditability.  
It also demonstrates the thought process of a **Senior Software Architect and Lead Developer**.

---

## 🎯 Vision

- Provide quick and transparent loan services to borrowers.  
- Enable lenders to invest and earn returns with a fair profit-sharing model.  
- Ensure trust through compliance, auditability, and strong risk management.  
- Support admins, CRM officers, and support staff with tools to manage and monitor the platform effectively.  

---

## 👥 Roles and Workflows

### Borrower
- Registers with personal and financial details.  
- Can apply for loans, repay in installments, and manage profile.  
- Has dashboard showing loan history, repayments, and notifications.  

### Lender
- Registers with legal, financial, and compliance details.  
- Can fund approved loans, earn interest (7%), and monitor portfolio.  
- Has dashboard showing total investments, profit, overdue repayments.  

### Credit Risk Manager (CRM)
- Approves or rejects Borrower and Lender registrations.  
- Reviews financial data, debts, and underwriting information.  
- Dashboard with Borrower and Lender details for vetting.  

### Admin
- Approves or rejects Loan applications.  
- Manages Borrowers, Lenders, CRM approvals, and loan lifecycle.  
- Earns platform fee (3% per loan).  
- Dashboard is a **superset of all roles** with global monitoring.  

### Support Staff
- Provides communication hub for issues between Borrowers, Lenders, CRM, and Admin.  
- Message dashboard to track and respond to cases.  

---

## 🧩 Core Entities

### Borrower Profile
- **Personal Information**: Name, DOB, Age, National ID, Address, Contact.  
- **Financial Information**: Employer, Job Title, Employment Length, Income, Additional Income.  
- **Credit & Debt**: List of debts (Lender, Type, Amount).  
- **Status**: Draft, Submitted, Approved, Rejected.  

### Lender Profile
- **Business Information**: Registration number, Incorporation docs, Licenses, Compliance statement.  
- **Financial Capacity**: Funding source, Description, Capital reserves.  
- **Risk Management**: Underwriting policy, Risk tools, Servicing strategy, Pricing strategy.  
- **Status**: Draft, Submitted, Approved, Rejected.  

### Loan Entity
A Loan is a **first-class aggregate root** with behaviours and lifecycle events.

**Core Identity**
- LoanId, BorrowerId, LenderId, ApplicationDate, Status.  

**Request Details**
- RequestedAmount, ApprovedAmount, Purpose, Term, RepaymentFrequency, InterestRate, Fees, TotalRepayableAmount.  

**Collateral**
- Type, Value, Description, Documents.  

**Underwriting & Risk**
- CreditScoreAtApplication, DebtToIncomeRatio, RiskGrade, Decision Notes.  

**Repayment Plan**
- Schedule (installments with due date, principal, interest, paid/unpaid).  
- NextPaymentDueDate, OverdueBalance, LateFees.  

**Lifecycle Events**
- LoanSubmitted, LoanApproved, LoanDeclined, LoanFunded, LoanActivated, PaymentMade, LoanCompleted, LoanDefaulted, LoanCancelled.  

**Audit & Meta**
- CreatedBy, CreatedAt, ModifiedBy, AuditTrail.  

---

## 🔔 Notifications and Messages

- **Messages**: direct communication between Borrower, Lender, Support, Admin.  
- **Notifications**: system-generated events (loan approved, funded, repayment overdue, etc.).  
- Each user sees alerts via dashboard and navbar badge.  

---

## 📊 Dashboards

- **Borrower Dashboard**: Loans applied, repayments, notifications.  
- **Lender Dashboard**: Loans funded, profit earned, overdue repayments, portfolio summary.  
- **Admin Dashboard**: Global view (Borrowers, Lenders, Loans, Profits).  
- **CRM Dashboard**: Pending Borrower/Lender registrations with vetting actions.  
- **Support Dashboard**: Messages and case resolution.  

---

## 💰 Business Model

- Borrower pays back loan with 10% interest.  
- Lender earns 7% return.  
- Platform/Admin retains 3% fee.  

---

## 🔐 Security & Compliance (Recommended Enhancements)

- **KYC/AML**: Verify Borrower and Lender identities, prevent fraud.  
- **GDPR/Data Protection**: Encrypt data at rest & in transit. Clear consent management.  
- **Role-Based Access Control (RBAC)**: Ensure least-privilege policies for Borrowers, Lenders, Admin, CRM.  
- **Secure Document Storage**: All uploaded files (licenses, collateral docs, income proof) stored securely in Blob Storage with access tokens.  

---

## 🔍 Auditability

- Immutable audit logs for loan approvals, declines, repayments.  
- Track **who, when, why** for every critical decision.  
- Event-sourcing approach possible for replay and compliance.  

---

## 🔄 Extended Loan Lifecycle

Additional states:  
- **PendingFunding** – approved but not yet funded.  
- **InArrears** – overdue payments but not defaulted.  
- **Restructured** – loan renegotiated with new terms.  

Support for:  
- **Partial Payments** – real-world repayment flexibility.  
- **Top-ups** – Borrowers can request more funds within the same loan entity.  

---

## 📈 Lender Portfolio Features

- Diversification insights (loans across risk grades).  
- Expected vs. actual profit tracking.  
- Overdue/default rate monitoring.  
- Encourage reinvestment with gamification (badges, levels).  

---

## 🛠 Technical Architecture

- **Backend**: ASP.NET 8 Web API, Clean Architecture (Domain, Application, Infrastructure, Persistence, Tests).  
- **CQRS**: Separate commands & queries, MediatR pipeline for logging, validation.  
- **Database**: InMemory & SQL implementations, with seeding.  
- **Infrastructure**: Azure App Services, Functions, Blob Storage, SQL/Cosmos DB, Service Bus/Event Grid.  
- **Logging & Exception Handling**: Centralized logging, structured exceptions, global error handling middleware.  
- **CI/CD**: GitHub Actions or Azure DevOps pipelines for build, test, deploy.  

---

## 🏠 Landing Page

Marketing-driven landing page:  
- Borrowers → "Get instant loans with easy approval."  
- Lenders → "Invest and earn steady returns."  

---

## ✅ Summary

MoneyMarket is designed not only as a working borrower-lender platform but as a **showcase of enterprise-level software engineering**.  
It demonstrates:  
- Strong **domain-driven modeling**.  
- Clear **role-based workflows**.  
- Event-driven **Loan lifecycle**.  
- Security, compliance, and auditability for fintech-grade robustness.  

---

© MoneyMarket – Designed as a demo project by Faz Ahmed (Senior Architect & Lead Developer).

