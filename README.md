
# MoneyMarket Platform

MoneyMarket is an online platform that connects **Borrowers** who need funds with **Lenders** who want to invest and earn profits.  
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

