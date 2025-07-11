# High-Quality, Secure, and Privacy-First Code

## Core Principles
- Write clean, concise, and readable code that prioritizes security and privacy.
- Follow the principle of least privilege: grant only the permissions necessary for functionality.
- Minimize data exposure: collect, process, and store only essential data.
- Use declarative and functional programming where applicable to reduce side effects.
- Avoid code duplication through modularization and reusable components.
- Use descriptive, self-explanatory naming conventions (e.g., is_enabled, has_access).
- Prefer lowercase with underscores for file and directory names (e.g., utils/security_utils).
- Document code intent and edge cases clearly using comments or external documentation.
- Adhere to language-specific style guides (e.g., PEP 8 for Python, Airbnb for JavaScript).
- Ensure code is maintainable, scalable, and testable.

## Security Guidelines
- Sanitize and validate all user inputs to prevent injection attacks (e.g., SQL, XSS, command injection).
- Use parameterized queries or prepared statements for database operations.
- Implement secure authentication and authorization (e.g., OAuth 2.0, JWT, role-based access control).
- Encrypt sensitive data at rest and in transit using strong, up-to-date cryptographic standards (e.g., AES-256, TLS 1.3).
- Avoid hardcoding secrets (e.g., API keys, passwords); use environment variables or secret management tools.
- Implement rate limiting and throttling to prevent abuse and denial-of-service attacks.
- Log security-relevant events (e.g., failed logins, privilege escalations) without exposing sensitive data.
- Regularly audit dependencies for vulnerabilities using tools like Dependabot or Snyk.
- Use secure defaults (e.g., disable unnecessary features, enforce HTTPS, restrict CORS).

## Privacy Guidelines
- Minimize data collection: only gather what is strictly necessary for functionality.
- Anonymize or pseudonymize personal data where possible.
- Implement data retention policies to delete unneeded data after a defined period.
- Provide clear user consent mechanisms for data collection and processing.
- Avoid logging sensitive user data (e.g., PII, passwords, tokens) in logs or error messages.
- Comply with privacy regulations (e.g., GDPR, CCPA) when handling user data.
- Use privacy-preserving techniques like differential privacy for analytics, if applicable.

## Code Structure and Organization
- Organize code into logical modules or packages based on functionality (e.g., auth, utils, models).
- Separate concerns (e.g., business logic, data access, presentation) to improve maintainability.
- Use consistent file naming and structure across the project.
- Export only necessary functions, classes, or modules to reduce surface area.
- Keep configuration files (e.g., .env, YAML) separate from source code.
- Use version control with meaningful commit messages and branch naming conventions.

## Error Handling and Validation
- Handle errors and edge cases at the start of functions or methods.
- Use early returns or guard clauses to avoid nested conditionals.
- Place the happy path at the end of functions for readability.
- Avoid unnecessary else clauses; prefer if-return patterns.
- Implement comprehensive input validation using schemas or type systems.
- Use custom error types or factories for consistent error handling.
- Provide user-friendly error messages without leaking sensitive information.
- Log errors with sufficient context for debugging, excluding sensitive data.

## Performance Optimization
- Minimize blocking operations; prefer asynchronous I/O where supported.
- Optimize resource usage (e.g., memory, CPU, network) through profiling and benchmarking.
- Implement caching for frequently accessed data using in-memory stores (e.g., Redis, Memcached).
- Use lazy loading for large datasets or expensive computations.
- Optimize data serialization/deserialization for efficiency.
- Avoid premature optimization; focus on measurable performance bottlenecks.

## Testing and Quality Assurance
- Write unit, integration, and end-to-end tests to cover critical functionality.
- Aim for high test coverage (>80%) while prioritizing meaningful tests.
- Use static analysis tools (e.g., linters, type checkers) to catch errors early.
- Perform security testing (e.g., penetration testing, fuzzing) for critical components.
- Automate testing and linting in CI/CD pipelines.
- Write tests that are independent, repeatable, and fast.

## Language-Agnostic Conventions
- Use type systems or type hints where supported to catch errors at compile time.
- Prefer immutable data structures to reduce unintended side effects.
- Avoid global state; use dependency injection or explicit parameter passing.
- Keep functions small and focused, ideally handling one responsibility.
- Use meaningful abstractions to hide implementation details.
- Avoid magic numbers or strings; use constants or enums instead.
- Handle concurrency safely with appropriate synchronization mechanisms (e.g., locks, atomic operations).

## Framework and Library Usage
- Use well-maintained, secure libraries and frameworks with active communities.
- Follow framework-specific best practices and conventions.
- Leverage dependency injection for managing shared resources and state.
- Use middleware or interceptors for cross-cutting concerns (e.g., logging, authentication).
- Avoid deprecated APIs or features; stay updated with the latest stable versions.

## Documentation
- Document public APIs, functions, and modules with clear usage examples.
- Include security and privacy considerations in documentation for sensitive components.
- Maintain a README with project setup, usage, and contribution guidelines.
- Keep documentation up-to-date with code changes using automated tools where possible.
- Use inline comments sparingly to explain complex logic or non-obvious decisions.

## Continuous Improvement
- Refactor code regularly to improve readability, performance, and maintainability.
- Conduct code reviews with a focus on security, privacy, and quality.
- Use automated tools for code formatting and style enforcement (e.g., Prettier, Black).
- Stay informed about emerging security threats and best practices.
- Encourage a culture of feedback and collaboration within the development team.

## References
- OWASP Secure Coding Practices: https://owasp.org/www-project-secure-coding-practices/
- Privacy by Design Principles: https://www.ipc.on.ca/wp-content/uploads/resources/7foundationalprinciples.pdf
- Language-specific style guides (e.g., PEP 8 for Python, Google Java Style Guide for Java).
- Framework-specific documentation for best practices and conventions.
