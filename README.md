# Foody
Foody is a full-stack E-commerce system for home-delivered food. Built with .NET 8 and React

# Tech Stack
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![Playwright](https://img.shields.io/badge/-Playwright-%232EAD33?style=for-the-badge&logo=playwright&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/postgresql-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![Stripe](https://img.shields.io/badge/Stripe-5469d4?style=for-the-badge&logo=stripe&logoColor=ffffff)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![PostNord](https://img.shields.io/badge/PostNord-004B87?style=for-the-badge&logo=postnord&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)

# Navigation
- [Tech Stack](#tech-stack)
- [Introduction](#introduction)
- [Preview](#preview)
- [Requirement Spec](#requirement-spec)
- [API Endpoints](#api-endpoints)
- [ER Diagram](#er-diagram)
- [How to run](#how-to-run)
- [License](#license)

# Introduction 
This project is part of my educational internship job where i follow a specific requirement spec which is to create an online ECommerce which inlcudes Frontend/Backend more like a fullstack project. 
I choose to create an food ordering system where you can create an account and login as user to later be able to get food home delivery. I Integrated with postnord as delivery option, stripe for payments and SendGrid for email conformation when order is finished.
OBS! Website language in Swedish

# Preview
<img width="2248" height="1199" alt="Screenshot 2026-02-20 105415" src="https://github.com/user-attachments/assets/d3d919a1-edeb-45f9-9df4-c1e49ff83911" />
<img width="1885" height="1119" alt="Screenshot 2026-02-20 112602" src="https://github.com/user-attachments/assets/86834531-2460-470e-95b8-59be6d314731" />
<img width="1237" height="1108" alt="Screenshot 2026-02-20 112331" src="https://github.com/user-attachments/assets/d4927657-08de-41d5-b5df-f3b5b2ebfbe5" />

# Requirement Spec
## Period 1                                                                           
| Id   | Requirement list                                   | Priority |     
|------|----------------------------------------------------|----------| 
| 1    | `Administration`                                   | 1 |
| 1.1  | `Login`                                            | 2 |
| 1.2  | `Products`                                         | 1 |
| 1.3  | `Orders`                                           | 2 |
| 1.4  | `Users (admins/customers)`                         | 3 |
| 2    | `Shop`                                             | 1 |
| 2.1  | `Header (menu, cart link, login)`                  | 1 |
| 2.2  | `Product list`                                     | 1 |
| 2.2.1| `Sort / filter (by category)`                      | 2 |
| 2.2.2| `Search`                                           | 3 |
| 2.2.3| `Pagination`                                       | 2 |
| 3    | `Product detail page`                              | 1 |
| 3.1  | `View product details`                             | 1 |
| 3.2  | `Add to cart`                                      | 1 |
| 4    | `Shopping cart`                                    | 1 |
| 4.1  | `List cart items`                                  | 1 |
| 4.2  | `Remove items`                                     | 2 |
| 4.3  | `Show prices / shipping cost / total`              | 1 |
| 5    | `Checkout / payment`                               | 1 |
| 5.1  | `Delivery address`                                 | 1 |
| 5.2  | `Klarna integration`                               | 2 |
| 5.3  | `PostNord integration`                             | 3 |
| 5.4  | `Thank you for your order page`                    | 1 |
| 6    | `My pages`                                         | 1 |
| 6.1  | `Order list`                                       | 2 |
| 6.2  | `Order details`                                    | 2 |
| 7    | `Other`                                            | 2 |
| 7.1  | `System logging`                                   | 2 |
| 7.2  | `Send order confirmation email`                    | 3 |
| 7.3  | `Unit tests / integration tests`                   | 3 |     

## Period 2 

## Performance & Scalability
Hybrid caching: To minimize database load and cloud costs, a cache layer is implemented at the database call level. Using Redis (via HybridCache), the system ensures high-speed data retrieval while managing the complexities of cache invalidation and distributed consistency.

Event-Driven Architecture: To ensure a loosely coupled and responsive system, long-running processes (like order exports and confirmation emails) are handled asynchronously. Using RabbitMQ and MassTransit, the system triggers order_created events to decouple the checkout flow from background processing.

## Infrastructure & Deployment
Containerization (Docker): The entire ecosystem—including the API, and external services—is containerized using Docker Compose. This ensures environment parity (it works on every machine) and provides a clear path for cloud scaling.

## CI/CD Pipeline
Quality and deployment are automated using GitHub Actions. Every change undergoes Continuous Integration (automated builds and tests) and Continuous Deployment to ensure the production environment is always stable and up to date.

## Quality Assurance
Automated Testing: Reliability is guaranteed through a multi-level testing strategy:

Unit & Integration Tests: Using xUnit and Testcontainers to validate business logic.

Isolated Infrastructure Testing: Utilizing Testcontainers to spin up ephemeral, isolated instances of your database (PostgreSQL) and message broker (RabbitMQ). This allows you to test against the exact same engine used in production—catching version-specific bugs—without ever touching or risking your production data.

# API Endpoints

## 🔐 Auth
| Method | Endpoint                          | Description                     | Status |
|--------|-----------------------------------|----------------------------------|--------|
| POST   | `/api/Auth/register`              | Register new user                | ✅ |
| POST   | `/api/Auth/login`                 | Login user                       | ✅ |
| PATCH  | `/api/Auth/update-profile`        | Update user profile              | ✅ |
| PUT    | `/api/Auth/change-password`       | Change user password             | ✅ |
| POST   | `/api/Auth/logout`                | Logout user                      | ✅ |
| PUT    | `/api/Auth/refreshToken`          | Refresh JWT token                | ✅ |

## 🗂 Category
| Method | Endpoint                                   | Description                          | Status |
|--------|--------------------------------------------|--------------------------------------|--------|
| GET    | `/api/Category/tree`                       | Get category tree                    | ✅ |
| GET    | `/api/Category/{id}`                       | Get category by id                   | ✅ |
| GET    | `/api/Category/subCategory/{id}`           | Get subcategories                    | ✅ |
| GET    | `/api/Category/subSubCategory/{id}`        | Get sub-subcategories                | ✅ |

## 🥗 NutritionValue
| Method | Endpoint                                           | Description                          | Status |
|--------|----------------------------------------------------|--------------------------------------|--------|
| POST   | `/api/NutritionValue/create`                       | Create nutrition value               | ✅ |
| GET    | `/api/NutritionValue/GetAllByProduct/{foodId}`     | Get nutrition by product             | ✅ |
| GET    | `/api/NutritionValue/{id}`                         | Get nutrition by id                  | ✅ |
| PUT    | `/api/NutritionValue/update`                       | Update nutrition value               | ✅ |
| DELETE | `/api/NutritionValue/delete/{id}`                  | Delete nutrition value               | ✅ |

## 🎁 Offer
| Method | Endpoint                    | Description              | Status |
|--------|-----------------------------|--------------------------|--------|
| POST   | `/api/Offer`                | Create offer             | ✅ |
| GET    | `/api/Offer`                | Get all offers           | ✅ |
| GET    | `/api/Offer/{id}`           | Get offer by id          | ✅ |
| DELETE | `/api/Offer/delete/{id}`    | Delete offer             | ✅ |

## 📦 Order
| Method | Endpoint                                | Description                      | Status |
|--------|-----------------------------------------|----------------------------------|--------|
| POST   | `/api/Order/create`                     | Create order                     | ✅ |
| GET    | `/api/Order/{id}`                       | Get order by id                  | ✅ |
| GET    | `/api/Order/MyOrders`                   | Get current user orders          | ✅ |
| GET    | `/api/Order/MyOrder{orderId}`           | Get specific user order          | ✅ |
| PATCH  | `/api/Order/update-order`               | Update order                     | ✅ |
| PUT    | `/api/Order/Cancel{orderId}`            | Cancel order                     | ✅ |
| POST   | `/api/Order/CalculateTax`               | Calculate tax                    | ✅ |
| POST   | `/api/Order/retrieve-payment-intent`    | Retrieve payment intent          | ✅ |

## 📮 PostNord
| Method | Endpoint                                   | Description                   | Status |
|--------|--------------------------------------------|-------------------------------|--------|
| POST   | `/api/Postnord/options`                    | Get shipping options          | ✅ |
| POST   | `/api/Postnord/postalCode/Validation`      | Validate postal code          | ✅ |

## 🛒 Product
| Method | Endpoint                       | Description                     | Status |
|--------|--------------------------------|----------------------------------|--------|
| POST   | `/api/Product/create`          | Create product                  | ✅ |
| GET    | `/api/Product/GetSome`         | Get paged products              | ✅ |
| GET    | `/api/Product/{id}`            | Get product by id               | ✅ |
| DELETE | `/api/Product/{id}`            | Delete product                  | ✅ |
| GET    | `/api/Product/details/{id}`    | Get product details             | ✅ |
| GET    | `/api/Product/filter`          | Filter products                 | ✅ |
| PUT    | `/api/Product/update`          | Update product                  | ✅ |
| GET    | `/api/Product/brands`          | Get brands                      | ✅ |

## 💳 Stripe
| Method | Endpoint                               | Description                     | Status |
|--------|----------------------------------------|----------------------------------|--------|
| POST   | `/api/Stripe/create-payment-intent`    | Create payment intent            | ✅ |

# ER Diagram
<img width="2050" height="1963" alt="Untitled" src="https://github.com/user-attachments/assets/3cbfcb74-647d-40ef-854f-09ce1e4607b9" />

# How to run
!Recommended Approach
1. Clone git repository
2. Have docker installed, in visual studio backend folder create env file at docker compose file located here Foody\Backend\docker-compose.dcproj. env file should look like this

.ENV

ASPNETCORE_ENVIRONMENT=Development

POSTGRES CONNECTIONSTRING

DefaultConnection=Host=postgres;Port=5432;Database=somename;Username=postgres;Password=somepass;Trust Server Certificate=True

JWT

Jwt__Issuer=

Jwt__Audience=

Jwt__Key=

Jwt__ExpireMinutes=

Jwt__RefreshDays=

STRIPE
Stripe__SecretKey=

SENDGRID
SendGrid__ApiKey=

POSTNORD
Postnord__ApiKey=

REDIS

Redis__ConnectionString=redis:6379

ALLOWED ORIGINS

AllowedOrigins__0=http://localhost:5173

AllowedOrigins__1=https://localhost:5173

CACHESETTINGS

CacheSettings__LongLivedMinutes=120

CacheSettings__ShortLivedMinutes=10

POSTGRES

POSTGRES_USER=postgres

POSTGRES_PASSWORD=somepass

PGADMIN

PGADMIN_DEFAULT_EMAIL=demo@example.com

PGADMIN_DEFAULT_PASSWORD=secret

RABBITMQ 

RABBITMQ_DEFAULT_USER=guest

RABBITMQ_DEFAULT_PASS=guest

RABBITMQ_DEFAULT_HOST=rabbitmq

RABBITMQ_URL=amqp://guest:guest@rabbitmq:5672/

Need to create your own SendGrid,Stripe,Postnord accounts for full experience once done run API 

3. FRONTEND in visual studio code run command in terminal npm run dev (Maybe need to install some dependencies if missing) (in vite.config.js comment out code if u dont have localhost-key-pem and localhost.pem for https take default code instead export default defineConfig({
  plugins: [react()],
}))
4. If everything is working ur done.

# License
MIT License – free to use, modify, and distribute.

