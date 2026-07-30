// Vector DB


//? How to store data in a vector database?
// - We can store data in a vector database by converting the data into vectors.
// - We can use various techniques to convert data into vectors, such as word embeddings, sentence embeddings, and document embeddings.
// - Once the data is converted into vectors, we can store the vectors in a vector database and use them for various applications, 
// such as search, recommendation, and clustering.

// Vectors:
//? What is a vector?
// - A vector is a mathematical object that has both magnitude and direction. In the context of machine learning and data science, 
// a vector is often represented as an array of numbers that can represent various types of data, such as text, images, or audio. 

// ? How to create a vector?
// - We can create a vector by using various techniques, such as word embeddings, sentence embeddings and document embeddings. 
// For example, we can use the Word2Vec algorithm to create word embeddings, which convert words into vectors based on their context in a corpus of text. 
// Similarly, we can use the Universal Sentence Encoder to create sentence embeddings, which convert sentences into vectors that capture their meaning. 
// Document embeddings can be created using techniques like Doc2Vec, which convert entire documents into vectors. 
// Once we have created the vectors, we can store them in a vector database for various applications.  
// 

//? Is there a limit to the number of vectors that can be stored in a vector database?
// - The limit to the number of vectors that can be stored in a vector database depends on the specific implementation and the resources available. 
// Some vector databases may have a limit on the number of vectors that can be stored, while others may allow for unlimited storage. 


//? How to insert vectors into a vector database?
// - We can insert vectors into a vector database by using the appropriate API or SDK provided by the database.
// - The process typically involves creating a connection to the database, preparing the vector data in the required format, 
// and then using the insert or add method to store the vectors in the database.  

//? Does Vector DB support multiple data types?
// - Yes, vector databases can support multiple data types, such as text, images, audio, and video.
// - The specific data types supported may vary depending on the implementation of the vector database.
// - For example, some vector databases may support only text data, while others may support multiple data types.
// - The ability to support multiple data types allows for more versatile applications, such as multimodal search and recommendation systems.


//? Does Vector DB also support multiple tables or vector collections or vector dimensions?
// - Yes, some vector databases support multiple tables or collections, allowing for better organization and management of vectors.
// - The support for multiple vector dimensions may vary depending on the implementation of the vector database.

//? What's the difference between a vector database and a traditional relational database?
// - A vector database is designed to store and manage high-dimensional vector data, while a traditional relational database 
// is designed to store and manage structured data in tables with rows and columns.
// - Vector databases are optimized for similarity search and retrieval of vectors, while relational databases are optimized 
// for querying and managing structured data.
// - Vector databases often use specialized indexing techniques, such as approximate nearest neighbor (ANN) search, to efficiently 
// retrieve similar vectors, while relational databases use indexing techniques like B-trees and hash indexes for efficient querying of structured data.  
// - In summary, vector databases are specialized for handling high-dimensional vector data, while traditional relational databases are 
// designed for structured data management.
// 


//? Difference between how the data is stored in a vector database and a traditional relational database
// - In a vector database, data is stored as high-dimensional vectors, which are typically represented as arrays of numbers.
// - In a traditional relational database, data is stored in tables with rows and columns, where each row represents a record and each column represents 
// a field or attribute of the record.  
// - The storage format of data in a vector database is optimized for similarity search and retrieval of vectors, 
// while the storage format of data in a traditional relational database is optimized for querying and managing structured data. 
// 


/*------------------------------------------------------------------------------------------------------------------------------*/



//? First: What is a Vector?

// A vector is simply a list of numbers.

// Example:

// [0.12, -0.45, 0.88, 0.23]

// Or:

// [0.234, 0.891, -0.112, ... 1536 numbers ...]

// By itself, this means nothing.

// The magic happens when an AI embedding model converts text into vectors.

// Example:

// "I love dogs"

// becomes

// [0.12, -0.45, 0.88, 0.23, ...]

// and

// "I like puppies"

// becomes

// [0.11, -0.42, 0.91, 0.25, ...]

// Notice they're close.

// The AI has transformed meaning into numbers.

// Mental Model

// Imagine a huge 3D space.

//             Cat
//              *
//             /
//            /
//           *
//        Kitten

// Dog *
//       \
//        \
//         *
//       Puppy

// Words with similar meaning end up near each other.

// In reality:

// not 3 dimensions
// 384 dimensions
// 768 dimensions
// 1536 dimensions
// 3072 dimensions

// depending on the embedding model.

// Why SQL Databases Struggle

// Suppose you have:

// Id	Document
// 1	I love dogs
// 2	I like puppies
// 3	My car is red

// Now user searches:

// animals people keep as pets

// SQL doesn't know:

// dogs ≈ pets
// puppies ≈ animals

// SQL only knows exact words.

// SELECT *
// FROM Documents
// WHERE Document LIKE '%animals%'

// returns nothing.

// What Vector DB Does

// Every document is converted to a vector.

// "I love dogs"
// → [0.12, -0.45, ...]

// "I like puppies"
// → [0.11, -0.42, ...]

// "My car is red"
// → [0.91, 0.23, ...]

// User query:

// animals people keep as pets

// also becomes a vector:

// [0.10, -0.44, ...]

// The vector DB finds vectors nearest to it.

// Result:

// "I love dogs"
// "I like puppies"

// even though those words don't exist in the query.

// This is called:

// Semantic Search

// Search by meaning instead of words.

// Relational DB vs Vector DB
// Feature	SQL DB	Vector DB
// Exact lookup	Excellent	Poor
// Joins	Excellent	No
// Transactions	Excellent	Limited
// Semantic search	Poor	Excellent
// Similarity search	Poor	Excellent
// Structured data	Excellent	Limited
// AI applications	Limited	Excellent
// Example

// SQL:

// SELECT *
// FROM Users
// WHERE Id = 10

// Vector DB:

// Find top 5 documents most similar to:

// "How do I deploy a .NET app?"
// What Actually Gets Stored?

// Usually:

// {
//   "id": "doc1",
//   "text": "How to deploy a .NET application on IIS",
//   "vector": [0.12, -0.45, ...],
//   "metadata": {
//       "category": "dotnet",
//       "author": "indar"
//   }
// }

// A vector DB stores:

// Original text
// Embedding vector
// Metadata
// Is Data Readable?

// Yes.

// Example:

// {
//   "id": "123",
//   "text": "Node.js worker threads tutorial",
//   "vector": [0.234, 0.872, -0.112, ...]
// }

// The text is readable.

// The vector itself is not meaningful to humans.

// Think of it like:

// Photo file

// You can see the photo.

// But the underlying binary bytes are not useful to read.

// Vector embeddings are similar.

// CRUD Operations
// Insert

// Generate embedding.

// const embedding = await openai.embeddings.create({
//     model: "text-embedding-3-small",
//     input: "How to deploy .NET on IIS"
// });

// Store:

// {
//    text: "How to deploy .NET on IIS",
//    vector: [...]
// }
// Read

// Convert query to embedding.

// "deploy asp.net application"

// ↓

// [0.11, -0.45, ...]

// Search nearest vectors.

// topK = 5

// Return:

// How to deploy .NET on IIS
// Deploying ASP.NET Core
// ...
// Update

// Change text.

// Old:
// How to deploy .NET on IIS

// New:
// How to deploy ASP.NET Core on IIS

// You must regenerate embedding.

// New Text
// ↓
// New Embedding
// ↓
// Update Record

// Unlike SQL:

// UPDATE Documents
// SET Title='new title'

// In vector DB, updating content often means recomputing vectors.

// Delete
// Delete vector by ID

// Same concept as SQL.

// Most Popular Vector Databases

// Dedicated vector DBs:

// Pinecone
// Weaviate
// Qdrant
// Milvus

// Traditional databases with vector support:

// PostgreSQL
// MongoDB
// Microsoft SQL Server

// Many companies now use PostgreSQL + pgvector instead of a dedicated vector DB.

// How RAG Uses a Vector DB

// Suppose you have 10,000 markdown files about your HR product.

// Without RAG:

// User:
// How is leave balance calculated?

// LLM:

// I don't know.

// With RAG:

// Step 1

// Convert all docs to vectors.

// Markdown
// ↓
// Embedding
// ↓
// Vector DB
// Step 2

// User asks:

// How is leave balance calculated?
// Step 3

// Query becomes vector.

// Step 4

// Vector DB returns most relevant chunks.

// Leave Balance Formula

// Carry Forward Rules

// Accrual Policy
// Step 5

// Send those chunks to the LLM.

// Context:
// <retrieved chunks>

// Question:
// How is leave balance calculated?

// Now the LLM answers accurately.

// The Biggest Misconception

// Many people think:

// Vector DB = Database storing AI data

// Not really.

// A better definition is:

// A Vector DB is a database optimized for finding semantically similar items using embeddings.

// It doesn't replace SQL databases.

// Most production systems use both:

// PostgreSQL
//     ↓
// User data
// Orders
// Permissions
// Settings

// Vector DB
//     ↓
// Knowledge Base
// Documents
// RAG
// Semantic Search
// Recommendations

// For your HR product (MyOneflow), a practical setup would be:

// MongoDB/PostgreSQL
//     ↓
// Jobs
// Applicants
// Employers
// Leave Requests

// Vector DB
//     ↓
// Policies
// Help Documents
// Training Material
// AI Assistant Knowledge
// Resume Search
// Job Matching

// That's where vector databases become extremely powerful.



//? Who decides the similarity between vectors? Cat is similar to kitten, Dog is similar to puppy, Does vector db decides this 
//? on the fly and we don't need to worry about where and how the relationship is being established or we give the co-ordinates in the 3d space.


// The Vector DB does NOT decide that Cat is similar to Kitten.

// The Embedding Model decides that.

// The Vector DB merely stores vectors and performs fast mathematical distance calculations.

// Think of it as 2 separate systems
// System 1: Embedding Model

// Examples:

// OpenAI text-embedding-3-small
// OpenAI text-embedding-3-large
// Sentence Transformers
// BGE
// E5
// Cohere Embeddings

// Its job:

// Text
//  ↓
// Meaning
//  ↓
// Vector

// Example:

// Cat

// becomes

// [0.12, 0.43, -0.91, ...]

// and

// Kitten

// becomes

// [0.13, 0.41, -0.89, ...]

// The embedding model intentionally places them close together.

// System 2: Vector DB

// The Vector DB receives:

// Cat    → [0.12, 0.43, -0.91]
// Kitten → [0.13, 0.41, -0.89]
// Dog    → [0.82, 0.21, 0.17]

// The Vector DB has no idea what a cat is.

// It only sees:

// Point A
// Point B
// Point C

// Its job is simply:

// Find nearest points

// Nothing more.

// Then Who Teaches Cat ≈ Kitten?

// The embedding model learns this during training.

// Imagine the model sees billions of sentences:

// Cats are common pets.

// Kittens are young cats.

// A kitten was adopted.

// The cat was sleeping.

// Over time it learns:

// Cat
// Kitten
// Pet
// Feline

// often appear in similar contexts.

// This comes from a famous NLP idea:

// "Words that appear in similar contexts tend to have similar meanings."

// Simple Example

// Suppose the internet only contains:

// Cat drinks milk.

// Kitten drinks milk.

// Dog chases ball.

// Puppy chases ball.

// The model observes:

// Word	Appears with
// Cat	milk
// Kitten	milk
// Dog	ball
// Puppy	ball

// It starts learning:

// Cat ≈ Kitten
// Dog ≈ Puppy

// because their contexts are similar.

// Are Coordinates Manually Assigned?

// No.

// Nobody writes:

// Cat = (1,2,3)
// Kitten = (1.1,2.1,3)

// The model learns the coordinates automatically.

// Think of training as:

// Billions of texts
//        ↓
// Training
//        ↓
// Learns relationships
//        ↓
// Creates embedding space

// Humans never define the coordinates.

// What Does the Space Actually Look Like?

// It's not really 3D.

// Imagine:

// Cat
// Kitten
// Lion
// Tiger

// cluster together.

// Dog
// Puppy
// Wolf

// cluster elsewhere.

// Car
// Truck
// Bike

// cluster in another region.

// Like this:

// Animals Region

// Cat
// Kitten
// Lion
// Tiger

// Dog
// Puppy
// Wolf


// Vehicles Region

// Car
// Truck
// Bike

// The model learned this structure automatically.

// How Does Vector DB Measure Similarity?

// Usually using:

// Cosine Similarity

// or

// Euclidean Distance

// For example:

// Cat    [1, 2, 3]
// Kitten [1.1, 2.1, 3.1]

// Distance:

// 0.17

// Very close.

// Cat    [1,2,3]
// Truck  [50,60,70]

// Distance:

// 100+

// Very far.


// The Vector DB only computes the math.

// ---

// # Real-Life RAG Example

// Suppose your knowledge base contains:

// ```text
// Annual Leave Policy

// Sick Leave Policy

// Deployment Guide

// .NET Hosting Guide

// Each document gets embedded.

// Now user asks:

// How do I host an ASP.NET application?

// The embedding model converts it into a vector.

// The Vector DB calculates:

// Distance to Annual Leave Policy = 0.95

// Distance to Sick Leave Policy = 0.89

// Distance to Deployment Guide = 0.31

// Distance to .NET Hosting Guide = 0.05

// Closest:

// .NET Hosting Guide

// So it returns that.

// Notice:

// Vector DB doesn't know ASP.NET.
// Vector DB doesn't know hosting.
// Vector DB doesn't know .NET.

// It only knows vector math.

// The embedding model encoded the meaning.

// A More Advanced Insight

// The coordinates are not random.

// Each dimension often captures some abstract feature learned during training.

// Imagine (oversimplified):

// Dimension 1 = Animalness
// Dimension 2 = Size
// Dimension 3 = Domesticness
// Dimension 4 = Technology
// Dimension 5 = Emotion
// ...

// Then:

// Cat
// [0.9, 0.2, 0.8, 0.0, 0.1]

// Kitten
// [0.95, 0.1, 0.9, 0.0, 0.1]

// Car
// [0.0, 0.8, 0.0, 0.9, 0.0]

// In reality, there are 100s or 1000s of dimensions, and humans usually don't know exactly what each dimension means.

// The model discovers them automatically.

// For someone learning RAG, the key mental model is:

// Documents
//    ↓
// Embedding Model
//    ↓
// Vectors (meaning encoded as coordinates)
//    ↓
// Vector DB
//    ↓
// Nearest-neighbor search
//    ↓
// Relevant documents
//    ↓
// LLM generates answer

// The embedding model creates the map; the vector database is just the GPS that finds nearby locations on that map.