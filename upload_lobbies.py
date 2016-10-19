import requests
#pip install requests
#http://docs.python-requests.org/en/latest/user/install/#install
import json
import os
import sys
from base64 import b64encode
 
 

def get_json_documents():
	data = []
	try:
		with open("lobby_data.json", 'r') as data_file:
			for line in data_file.readlines():
				data.append(line)
	except:
		print("Error opening json document")
		print("Unexpected error:", sys.exc_info()[0])
			
	return data
def main():
	
	documents_list = get_json_documents()
	
	print("Batch inserting documents")
	batchInsert(documents_list)
	print("finished inserting")
 
#We'll use this function to insert our documents
def batchInsert(documents):
	#GameSparks User and pass
	userPass = b64encode(b"kbaxtrom1019@gmail.com:sprinter0*").decode("ascii")
	#The gameId we wish to insert documents into
	gameId = "z303174aoR8P"
	#The stage we wish to insert our documents into
	stage = "preview"
	#The collection we wish to insert these documents into
	collection = "script.lobbies"
	#This is the url for mo
	postURL = "https://portal.gamesparks.net/rest/games/" + gameId + "/mongo/" + stage + "/" + collection + "/insert"
	#Our generated auth token
	auth =  {"Authorization" : "Basic " + userPass}
	
	for x in documents:
		postData = requests.post(postURL, headers = auth, data = {"document" : x})
		print (postData.json())
		print("insert successful")



if __name__ == "__main__":
	main()