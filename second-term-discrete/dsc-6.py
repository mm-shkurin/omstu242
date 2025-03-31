def floyd_warshall(graph):
    num_vertices = len(graph)
    distance = [[float('inf')] * num_vertices for _ in range(num_vertices)]
    for i in range(num_vertices):
        for j in range(num_vertices):
            distance[i][j] = graph[i][j]
    for k in range(num_vertices):
        for i in range(num_vertices):
            for j in range(num_vertices):
                if distance[i][j] > distance[i][k] + distance[k][j]:
                    distance[i][j] = distance[i][k] + distance[k][j]
    return distance
def main():
    num_vertices = int(input("Enter the number of vertices:"))
    graph = []
    print("Enter the adjacency matrix (enter 'inf' to indicate the absence of an edge):")
    for i in range(num_vertices):
        row = input(f"String {i + 1}: ").strip().split()
        graph_row = []
        for value in row:
            if value == 'inf':
                graph_row.append(float('inf'))  
            else:
                graph_row.append(int(value))
        graph.append(graph_row)
    shortest_paths = floyd_warshall(graph)
    print("The matrix of shortest distances:")
    for row in shortest_paths:
        print(' '.join(['inf' if x == float('inf') else str(x) for x in row]))
if __name__ == "__main__":
    main()
