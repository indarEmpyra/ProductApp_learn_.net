// Learn AI

//? Types:
// 1. API based AI: This type of AI relies on external APIs to perform tasks. 
// It can be integrated into applications to provide functionalities like natural language processing, image recognition, and more. 
// Examples include OpenAI's GPT models, Google Cloud AI, and Microsoft Azure Cognitive Services.

// 2. RAG:
// RAG stands for Retrieval-Augmented Generation. It is a type of AI that combines retrieval-based methods with generative models.
// In RAG, the model retrieves relevant information from a knowledge base or database and uses that information to generate responses or outputs. 
// This approach enhances the model's ability to provide accurate and contextually relevant answers, especially in scenarios where the model's
// training data may be limited or outdated.  

// In simple words: 
// We can save instructions as .md files or .pdf files and then convert them into embeddings and save them in a vector database. 
// Now, when a user asks a question, we can retrieve the relevant instructions from the vector database and append them to the 
// prompt or context for the AI model to generate a more accurate and contextually relevant response. 

//3. Agent based AI: 
// This type of AI involves creating autonomous agents that can perform tasks, make decisions, and interact with their environment.


//? API based AI usage:
// Example: Using OpenAI's GPT model to generate jobDescription based on job title. 
// The application sends a request to the API with the user's input, and the API returns a generated response.  

//# NodeJS example:
// const { Configuration, OpenAIApi } = require("openai");
// const configuration = new Configuration({
//   apiKey: process.env.OPENAI_API_KEY,
// });
// const openai = new OpenAIApi(configuration);
// 
// async function generateJobDescription(jobTitle) {
//   const response = await openai.createChatCompletion({
//     model: "gpt-4",
//     messages: [
//       { role: "system", content: "You are a helpful assistant." },
//       { role: "user", content: `Generate a job description for the following job title: ${jobTitle}` },
//     ],
//   });
//   return response.data.choices[0].message.content;
// }


//# .Net example:
// using OpenAI_API;
// using OpenAI_API.Chat;
// var api = new OpenAIAPI("YOUR_API_KEY");
// var chatRequest = new ChatRequest
// {
//     Model = "gpt-4",
//     Messages = new List<ChatMessage>
//     {
//        new ChatMessage(ChatMessageRole.System, "You are a helpful assistant."),
//        new ChatMessage(ChatMessageRole.User, $"Generate a job description for the following job title: {jobTitle}")
//     }  
// };
// var chatResponse = await api.Chat.CreateChatCompletionAsync(chatRequest);
// var jobDescription = chatResponse.Choices[0].Message.Content;
// 
// 


//? RAG usage:
// Example: Using RAG to generate a response based on retrieved information from a vector database.

// In this example, we have a vector database that contains embeddings of various documents. 
// When a user asks a question, we retrieve the relevant documents from the vector database based on the user's query, 
// and then we use that retrieved information to generate a response using a generative model like GPT

// The process involves the following steps:
// 1. Convert the user's query into an embedding using the same method used to create the embeddings for the documents in the vector database.
// 2. Retrieve the most relevant documents from the vector database based on the similarity of the query embedding to the document embeddings.
// 3. Append the retrieved documents to the prompt or context for the generative model.
// 4. Generate a response using the generative model, which now has access to the relevant information from the retrieved documents.  


//# How to create instructions for RAG:
// .md files or .pdf files can be used to create instructions for RAG. 
// These files can contain detailed information as a plain text, guidelines, or any relevant content that can be converted into 
// embeddings and stored in a vector database.

//# Example of creating instructions for RAG:

//# Create a .md file named "job_description_instructions.md" with the following content:
/*
# Job Description Instructions
To generate a job description, please follow these guidelines:
1. Start with a clear and concise job title.
2. Provide a brief overview of the company and its mission.
3. List the key responsibilities and duties of the role.
4. Include the required qualifications and skills.
5. Mention any preferred qualifications or experience.
6. End with a call to action for potential candidates to apply.
*/

//#. How to convert the content of the .md file into embeddings using a suitable method (e.g., using OpenAI's embedding API).
// Example:
// const { Configuration, OpenAIApi } = require("openai");
// const configuration = new Configuration({
//   apiKey: process.env.OPENAI_API_KEY,
// });
// const openai = new OpenAIApi(configuration);

// async function createEmbeddings(content) {
//   const response = await openai.createEmbedding({
//     model: "text-embedding-3-large",
//     input: content
//   });
//   return response.data.data[0].embedding;
// }

//# How does an embedding work? An embedding is a numerical representation of text that captures its semantic meaning.
// It is generated by passing the text through a neural network model that has been trained on a large corpus of text data.
// The resulting embedding is a high-dimensional vector that can be used to compare the similarity between different pieces of text.
// For example, two pieces of text that have similar meanings will have embeddings that are close together in the vector space,
// while two pieces of text that have different meanings will have embeddings that are far apart.

// Examples of embeddings:
// 1. "Software Engineer" and "Developer" will have similar embeddings because they are related concepts in the field of technology.
// 2. "Software Engineer" and "Chef" will have different embeddings because they are unrelated concepts in different fields.


// Example of format of embeddings:
// Embedding for "Software Engineer": [0.123, 0.456, 0.789, ...]
// Embedding for "Developer": [0.124, 0.457, 0.788, ...]
// Embedding for "Chef": [0.987, 0.654, 0.321, ...]


//# How to save embeddings in a vector database:
// To save embeddings in a vector database, we can use a suitable vector database solution such as  
// Pinecone, Weaviate, or Milvus. These databases are designed to efficiently store and retrieve high-dimensional vectors.

// Example of saving embeddings in a vector database:
// const { PineconeClient } = require("@pinecone-database/pinecone");
// const pinecone = new PineconeClient({ apiKey: process.env.PINECONE_API_KEY });
// async function saveEmbedding(id, embedding) {
//   await pinecone.upsert({
//     indexName: "my-index",
//     vectors: [
//       {
//            id: id,
//            values: embedding
//       }
// ]
//   });
// }

//# How to retrieve embeddings from a vector database:
// To retrieve embeddings from a vector database, we can use a similarity search query to find the most similar embeddings to a given query embedding.
// Example of retrieving embeddings from a vector database:
// async function retrieveEmbeddings(queryEmbedding, topK) {
//   const response = await pinecone.query({
//     indexName: "my-index",
//     query: {
//       vector: queryEmbedding,
//       topK: topK
//     }
//   });
//   return response.matches;
// }

//# How to use retrieved embeddings to generate a response:
// Once we have retrieved the most relevant embeddings from the vector database, we can use them to generate a response using a generative model like GPT.
// Example of using retrieved embeddings to generate a response:
// async function generateResponse(queryEmbedding, topK) {
//   const retrievedEmbeddings = await retrieveEmbeddings(queryEmbedding, topK);
//   const context = retrievedEmbeddings.map(match => match.metadata.content).join("\n");
//  const response = await openai.createChatCompletion({
//     model: "gpt-4",
//      messages: [
//        { role: "system", content: "You are a helpful assistant." },
//        { role: "user", content: `Based on the following context, please answer the user's question: ${context}` },
//      ],
//     });
//     return response.choices[0].message.content;
// }



//# How to compare embeddings:
// To compare embeddings, we can use a similarity metric such as cosine similarity.

// How does the comparison of embeddings work of Software Engineer and Developer? 
// The comparison of embeddings works by calculating the cosine similarity between the two vectors.
// Cosine similarity is a measure of similarity between two non-zero vectors of an inner product space that measures the cosine of the angle between them.
// The cosine similarity is calculated as follows:
// Cosine Similarity = (A . B) / (||A|| * ||B||)
// Where A and B are the two vectors, (A . B) is the dot product of the vectors, and ||A|| and ||B|| are the magnitudes of the vectors.
// The result of the cosine similarity will be a value between -1 and 1, where 1 indicates that the vectors are identical,
// 0 indicates that the vectors are orthogonal (unrelated), and -1 indicates that the vectors are diametrically opposed (opposite meanings).
// In the case of "Software Engineer" and "Developer", the cosine similarity will be close to 1, indicating that they are similar concepts.
// In the case of "Software Engineer" and "Chef", the cosine similarity will be close to 0, indicating that they are unrelated concepts.
// In the case of "Software Engineer" and "Doctor", the cosine similarity will be close to 0, indicating that they are unrelated concepts.
// In the case of "Software Engineer" and "Teacher", the cosine similarity will be close to 0, indicating that they are unrelated concepts.
// In the case of "Software Engineer" and "Nurse", the cosine similarity will be close to 0, indicating that they are unrelated concepts.


//# How to use RAG in a real-world application:
// In a real-world application, RAG can be used to enhance the capabilities of AI models by providing them with access to relevant 
// information from a knowledge base or database.
// For example, in a customer support application, RAG can be used to retrieve relevant information from a knowledge base of frequently 
// asked questions and use that information to generate accurate and contextually relevant responses to customer inquiries.  

//# Do we need to convert instructions into embeddings for RAG manually?  
// No, we can automate the process of converting instructions into embeddings for RAG.
// We can create a script or a function that reads the content of the .md or .pdf files, converts the content into embeddings using a suitable method 
// (e.g., using OpenAI's embedding API), and saves the embeddings in a vector database.    

//# How to automate the process of converting instructions into embeddings for RAG:
// We can create a script or a function that reads the content of the .md or .pdf files, converts the content into embeddings using a suitable method 
// (e.g., using OpenAI's embedding API), and saves the embeddings in a vector database.
// Example of automating the process of converting instructions into embeddings for RAG:
// async function automateEmbeddingConversion(filePath) {
//   const content = await readFile(filePath, 'utf-8');
//   const embedding = await createEmbeddings(content);
//   await saveEmbedding(filePath, embedding);
// }

//# Is it possible to use RAG without a vector database?
// While it is technically possible to use RAG without a vector database, it is not recommended. 
// A vector database is specifically designed to efficiently store and retrieve high-dimensional vectors, 
// which is essential for the RAG approach. Without a vector database, the retrieval process would be less efficient and may not scale well with large datasets. 


//# Is it possible to just store instructions in a database and retrieve them without converting them into embeddings?
// Yes, it is possible to store instructions in a database and retrieve them without converting them into embeddings. 
// However, this approach may not be as effective as using embeddings for retrieval.
// Storing instructions in a database without converting them into embeddings would require a different retrieval mechanism, 
// such as keyword-based search or full-text search. While this approach may work for simple queries, it may not be as effective 
// for more complex queries that require understanding the semantic meaning of the instructions.  

//# If we store instructions in a database without converting them into embeddings, how would the retrieval process work?
// If we store instructions in a database without converting them into embeddings, the retrieval process would typically involve a 
// keyword-based search or full-text search mechanism.
// For example, if a user asks a question, the system would search the database for instructions  that contain keywords related to the user's query.
// The system would then retrieve the relevant instructions based on the presence of those keywords and use them to generate a response. 
// However, this approach may not be as effective as using embeddings for retrieval, as it may not capture the semantic meaning of 
// the instructions and may return irrelevant results if the keywords are not present in the instructions.  


//! If we don't retrieve or don't efficiently retrieve instructions for RAG, we will end up sending 
//! all whole instructions to the generative model, 
//! which can lead to increased latency and higher costs, as well as potentially overwhelming the model 
//! with too much information.
